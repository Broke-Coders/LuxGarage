import { RentalService } from "../rentalService.js";
import { statusBadge }   from "./utils/statusHelpers.js";

const API_ORIGIN = "http://localhost:5054";
let allRentals = [];

export function initRentals() {
  const tableBody = document.getElementById("rentalsTableBody");
  if (tableBody) {
    tableBody.addEventListener("click", async (e) => {
      const button = e.target.closest(".btn-action-cancel");
      if (button) {
        const rentalId = button.getAttribute("data-id");
        if (confirm(`Are you sure you want to cancel rental #${rentalId}?`)) {
          try {
            await RentalService.cancelRental(rentalId);
            loadRentals();
          } catch (err) {
            alert("Error cancelling rental: " + err.message);
          }
        }
      }
    });
  }
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
          <div class="td-vehicle-info">
            ${r.vehicleImageUrl 
              ? `<img src="${API_ORIGIN}${r.vehicleImageUrl}" class="offer-thumb" alt="${r.vehicleBrand} ${r.vehicleModel}" onerror="this.style.display='none'; this.nextElementSibling.style.display='flex';" />` 
              : ''
            }
            <div class="offer-thumb-placeholder" style="${r.vehicleImageUrl ? 'display: none;' : ''}">
              <ion-icon name="car-outline"></ion-icon>
            </div>
            <div>
              <strong>${r.vehicleBrand} ${r.vehicleModel}</strong>
              <span class="td-sub">${r.vehicleLicensePlate || 'N/A'} (ID: ${r.vehicleId})</span>
            </div>
          </div>
        </td>
        <td>
          <div class="td-customer">
            <ion-icon name="person-circle-outline"></ion-icon>
            <div>
              <strong>${r.customerFirstName || r.customerLastName ? `${r.customerFirstName} ${r.customerLastName}` : `Customer #${r.customerId}`}</strong>
              <span class="td-sub">${r.customerEmail || 'No email'}</span>
            </div>
          </div>
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
            ${(r.status === 'Pending' || r.status === 'ReservedWaitingForPayment') 
              ? `<button class="btn-action-cancel" data-id="${r.id}">
                   <ion-icon name="close-circle-outline"></ion-icon>
                   Cancel
                 </button>`
              : '<span class="action-none">—</span>'
            }
          </div>
        </td>
      </tr>`
    )
    .join("");
}
