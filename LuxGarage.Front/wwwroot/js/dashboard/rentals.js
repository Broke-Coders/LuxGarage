import { RentalService } from "../rentalService.js";
import { statusBadge }   from "./utils/statusHelpers.js";

let allRentals = [];

export function initRentals() {
  loadRentals();
}

export async function loadRentals() {
  const tableBody  = document.getElementById("rentalsTableBody");
  const tableEmpty = document.getElementById("rentalsTableEmpty");

  if (!tableBody) return;

  tableBody.innerHTML = `
    <tr class="table-loading-row">
      <td colspan="7">
        <div class="table-spinner">
          <div class="spinner"></div><span>Loading rentals…</span>
        </div>
      </td>
    </tr>`;
  
  if (tableEmpty) tableEmpty.style.display = "none";

  try {
    allRentals = await RentalService.getAllRentals();
    _renderTable(allRentals);
  } catch (e) {
    tableBody.innerHTML = `
      <tr><td colspan="7" style="text-align:center;padding:3rem;color:#c0392b;">
        ${e.message}
      </td></tr>`;
  }
}

function _renderTable(rentals) {
  const tableBody  = document.getElementById("rentalsTableBody");
  const tableEmpty = document.getElementById("rentalsTableEmpty");

  if (!rentals.length) {
    tableBody.innerHTML = "";
    if (tableEmpty) tableEmpty.style.display = "block";
    return;
  }
  if (tableEmpty) tableEmpty.style.display = "none";

  tableBody.innerHTML = rentals
    .map(
      (r) => `
      <tr>
        <td>#${r.id}</td>
        <td>
          <strong>${r.vehicleBrand} ${r.vehicleModel}</strong>
          <span class="td-sub">ID: ${r.vehicleId}</span>
        </td>
        <td>
          <strong>Customer #${r.customerId}</strong>
        </td>
        <td>
          <div class="td-date-range">
            <span>${new Date(r.startingTime).toLocaleDateString()}</span>
            <ion-icon name="arrow-forward-outline"></ion-icon>
            <span>${new Date(r.appointedReturnTime).toLocaleDateString()}</span>
          </div>
        </td>
        <td class="td-price">${Number(r.totalPrice).toLocaleString("pl-PL")} PLN</td>
        <td>${statusBadge(r.status)}</td>
        <td>
          <div class="td-actions">
             <!-- Future actions like 'Complete' or 'Mark as Paid' can go here -->
          </div>
        </td>
      </tr>`
    )
    .join("");
}
