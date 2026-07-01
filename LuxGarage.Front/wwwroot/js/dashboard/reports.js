import { RentalService } from "../rentalService.js";
import { CarService } from "../carService.js";

let robotoFontBase64 = null;

async function getRobotoBase64() {
  if (robotoFontBase64) return robotoFontBase64;
  try {
    const res = await fetch("https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/fonts/Roboto/Roboto-Regular.ttf");
    if (!res.ok) throw new Error("Network error");
    const arrayBuffer = await res.arrayBuffer();
    let binary = '';
    const bytes = new Uint8Array(arrayBuffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    robotoFontBase64 = window.btoa(binary);
    return robotoFontBase64;
  } catch (e) {
    console.warn("Failed to load Roboto font for PDF", e);
    return null;
  }
}

export function initReports() {
  const btnGenerate = document.getElementById("btn-generate-report");
  if (btnGenerate) {
    btnGenerate.addEventListener("click", generateReport);
  }
  loadVehiclesForDropdown();
}

async function loadVehiclesForDropdown() {
  try {
    const vehicles = await CarService.getAllCars();
    const select = document.getElementById("report-vehicle");
    if (select) {
      vehicles.forEach(v => {
        const option = document.createElement("option");
        option.value = v.id;
        option.textContent = `${v.brand} ${v.licensePlate}`;
        select.appendChild(option);
      });
    }
  } catch (e) {
    console.error("Failed to load vehicles for report filters", e);
  }
}

async function generateReport() {
  const btn = document.getElementById("btn-generate-report");
  const spinner = btn.querySelector(".btn-spinner");
  
  // Set loading
  btn.disabled = true;
  if (spinner) spinner.style.display = "inline-flex";

  try {
    // 1. Get filter values
    const dateFromStr = document.getElementById("report-date-from").value;
    const dateToStr = document.getElementById("report-date-to").value;
    const vehicleId = document.getElementById("report-vehicle").value;

    const includeCustomer = document.getElementById("report-content-customer").checked;
    const includePrice = document.getElementById("report-content-price").checked;
    const includeStatus = document.getElementById("report-content-status").checked;

    // 2. Fetch all rentals from existing API
    const allRentals = await RentalService.getAllRentals();
    let rentals = [...allRentals];

    // 3. Apply filters on the frontend
    if (dateFromStr) {
      const from = new Date(dateFromStr);
      from.setHours(0, 0, 0, 0);
      rentals = rentals.filter(r => {
        const start = r.startingTime || r.StartingTime;
        return start ? new Date(start) >= from : false;
      });
    }

    if (dateToStr) {
      const to = new Date(dateToStr);
      to.setHours(23, 59, 59, 999);
      rentals = rentals.filter(r => {
        const start = r.startingTime || r.StartingTime;
        return start ? new Date(start) <= to : false;
      });
    }

    if (vehicleId) {
      rentals = rentals.filter(r => {
        const vid = r.vehicleId || r.VehicleId;
        return String(vid) === String(vehicleId);
      });
    }

    // 3.5 Fetch Roboto Font for Polish Characters
    const fontBase64 = await getRobotoBase64();

    // 4. Generate PDF
    createPdf(rentals, allRentals.length, {
      includeCustomer,
      includePrice,
      includeStatus,
      dateFromStr,
      dateToStr,
      fontBase64
    });

  } catch (err) {
    alert("Error generating report: " + err.message);
  } finally {
    btn.disabled = false;
    if (spinner) spinner.style.display = "none";
  }
}

function createPdf(rentals, totalFetched, options) {
  const { jsPDF } = window.jspdf;
  const doc = new jsPDF();

  if (options.fontBase64) {
    doc.addFileToVFS("Roboto-Regular.ttf", options.fontBase64);
    doc.addFont("Roboto-Regular.ttf", "Roboto", "normal");
    doc.addFont("Roboto-Regular.ttf", "Roboto", "bold"); // using same for bold fallback
    doc.setFont("Roboto");
  }
  
  const fontFamily = options.fontBase64 ? "Roboto" : "helvetica";

  // Color Palette & Styling based on LuxGarage theme
  const primaryColor = [71, 69, 62]; // #47453e
  const textColor = [71, 69, 62];
  const accentColor = [141, 138, 124]; // #8d8a7c
  
  // Title
  doc.setTextColor(...primaryColor);
  doc.setFontSize(22);
  doc.setFont(fontFamily, "bold");
  doc.text("LuxGarage - Rental Report", 14, 22);

  // Subtitle / Filters Info
  doc.setFontSize(11);
  doc.setTextColor(...accentColor);
  doc.setFont(fontFamily, "normal");
  let filterText = `Generated on: ${new Date().toLocaleDateString()}`;
  if (options.dateFromStr || options.dateToStr) {
    filterText += ` | Period: ${options.dateFromStr || 'Any'} - ${options.dateToStr || 'Any'}`;
  }
  doc.text(filterText, 14, 30);
  
  // Prepare Table Data
  const head = [["ID", "Vehicle", "Rental Period"]];
  if (options.includeCustomer) head[0].push("Customer");
  if (options.includePrice) head[0].push("Total Price (PLN)");
  if (options.includeStatus) head[0].push("Status");

  const body = rentals.map(r => {
    const brand = r.vehicleBrand || r.VehicleBrand || '';
    const model = r.vehicleModel || r.VehicleModel || '';
    const plate = r.vehicleLicensePlate || r.VehicleLicensePlate || 'N/A';
    const start = r.startingTime || r.StartingTime;
    const end = r.appointedReturnTime || r.AppointedReturnTime;
    const id = r.id || r.Id;

    const row = [
      `#${id}`,
      `${brand} ${model} (${plate})`,
      `${new Date(start).toLocaleDateString()} - ${new Date(end).toLocaleDateString()}`
    ];

    if (options.includeCustomer) {
      const fName = r.customerFirstName || r.CustomerFirstName || '';
      const lName = r.customerLastName || r.CustomerLastName || '';
      const email = r.customerEmail || r.CustomerEmail || '';
      const cId = r.customerId || r.CustomerId;
      const name = fName || lName ? `${fName} ${lName}` : `ID: ${cId}`;
      row.push(`${name}\n${email}`);
    }
    
    if (options.includePrice) {
      const price = r.totalPrice || r.TotalPrice || 0;
      row.push(Number(price).toLocaleString("pl-PL"));
    }

    if (options.includeStatus) {
      const status = r.status || r.Status || '';
      row.push(status);
    }

    return row;
  });

  if (rentals.length === 0) {
    doc.setFontSize(14);
    doc.setTextColor(...primaryColor);
    doc.text(`No rentals match the selected filters.`, 14, 50);
    doc.setFontSize(10);
    doc.text(`(Fetched ${totalFetched} rentals from database)`, 14, 58);
  } else {
    // Generate Table
    doc.autoTable({
      startY: 38,
      head: head,
      body: body,
      theme: 'grid',
      styles: {
        font: fontFamily,
        fontSize: 10,
        textColor: textColor,
        lineColor: [224, 221, 208], // #e0ddd0
        lineWidth: 0.1,
      },
      headStyles: {
        fillColor: primaryColor,
        textColor: [235, 230, 206], // #ebe6ce
        fontStyle: 'bold',
      },
      alternateRowStyles: {
        fillColor: [250, 249, 244] // #faf9f4
      },
      margin: { top: 38 }
    });
    
    // Add Summary
    if (options.includePrice) {
      const totalRevenue = rentals.reduce((sum, r) => sum + Number(r.totalPrice), 0);
      const finalY = doc.lastAutoTable.finalY || 38;
      doc.setFontSize(12);
      doc.setFont(fontFamily, "bold");
      doc.setTextColor(...primaryColor);
      doc.text(`Total Revenue: ${totalRevenue.toLocaleString("pl-PL")} PLN`, 14, finalY + 10);
      doc.text(`Total Rentals: ${rentals.length}`, 14, finalY + 16);
    }
  }

  // Save PDF
  doc.save(`LuxGarage_Report_${new Date().getTime()}.pdf`);
}
