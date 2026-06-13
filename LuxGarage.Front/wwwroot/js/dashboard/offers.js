import { OfferService }                      from "../offerService.js";
import { CarService }                        from "../carService.js";
import { openModal, closeModal, setLoading,
         showError, clearError }             from "./utils/modal.js";

let allOffers       = [];
let pendingDeleteId = null;

export function initOffers() {
  _bindAddModal();
  _bindEditForm();
  _bindDeleteConfirm();
  loadOffers();
}

// ─── Load & render ───────────────────────────────────────────
export async function loadOffers(params = {}) {
  const tableBody  = document.getElementById("offersTableBody");
  const tableEmpty = document.getElementById("offersTableEmpty");

  tableBody.innerHTML = `
    <tr class="table-loading-row">
      <td colspan="7">
        <div class="table-spinner">
          <div class="spinner"></div><span>Loading offers…</span>
        </div>
      </td>
    </tr>`;
  tableEmpty.style.display = "none";

  try {
    allOffers = await OfferService.getAllOffers(params);
    _renderTable(allOffers);
  } catch (e) {
    tableBody.innerHTML = `
      <tr><td colspan="7" style="text-align:center;padding:3rem;color:#c0392b;">
        ${e.message}
      </td></tr>`;
  }
}

function _renderTable(offers) {
  const tableBody  = document.getElementById("offersTableBody");
  const tableEmpty = document.getElementById("offersTableEmpty");

  if (!offers.length) {
    tableBody.innerHTML = "";
    tableEmpty.style.display = "block";
    return;
  }
  tableEmpty.style.display = "none";

  tableBody.innerHTML = offers
    .map((o) => `
      <tr>
        <td>#${o.id}</td>
        <td class="td-offer-img">
          ${o.primaryImageUrl
            ? `<img src="${o.primaryImageUrl}" alt="${o.title}" class="offer-thumb" />`
            : `<div class="offer-thumb-placeholder"><ion-icon name="car-outline"></ion-icon></div>`
          }
        </td>
        <td>
          <strong>${o.title}</strong>
          <span class="td-sub">${o.brand} ${o.model}</span>
        </td>
        <td>${o.mileage}</td>
        <td class="td-price">${Number(o.price).toLocaleString("pl-PL")} PLN<span class="td-sub">/ day</span></td>
        <td>
          <div class="td-actions">
            <button class="btn-icon btn-icon--edit"   data-id="${o.id}" title="Edit">
              <ion-icon name="create-outline"></ion-icon>
            </button>
            <button class="btn-icon btn-icon--delete" data-id="${o.id}" title="Delete">
              <ion-icon name="trash-outline"></ion-icon>
            </button>
          </div>
        </td>
      </tr>`)
    .join("");

  tableBody.querySelectorAll(".btn-icon--edit").forEach((btn) =>
    btn.addEventListener("click", () => _openEditModal(parseInt(btn.dataset.id)))
  );
  tableBody.querySelectorAll(".btn-icon--delete").forEach((btn) =>
    btn.addEventListener("click", () => _openDeleteModal(parseInt(btn.dataset.id)))
  );
}

// ─── Add modal ───────────────────────────────────────────────
function _bindAddModal() {
  document.getElementById("btnAddOffer").addEventListener("click", async () => {
    document.getElementById("formAddOffer").reset();
    clearError("formAddOfferError");
    await _populateVehicleSelect("addOfferVehicleId");
    openModal("modalAddOffer");
  });

  document.getElementById("formAddOffer").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearError("formAddOfferError");
    const btn = document.getElementById("btnAddOfferSubmit");
    setLoading(btn, true);

    try {
      const form = e.target;
      await OfferService.createOffer({
        VehicleId:           parseInt(form.querySelector("#addOfferVehicleId").value),
        Title:               form.querySelector("#addOfferTitle").value.trim(),
        Description:         form.querySelector("#addOfferDescription").value.trim() || null,
        InitialPricePerDay:  parseFloat(form.querySelector("#addOfferPrice").value),
      });
      closeModal("modalAddOffer");
      await loadOffers();
    } catch (err) {
      showError("formAddOfferError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ─── Edit modal ──────────────────────────────────────────────
async function _openEditModal(id) {
  const offer = allOffers.find((o) => o.id === id);
  if (!offer) return;

  // Pobierz szczegóły oferty (OfferDetailsResponse ma więcej pól niż OfferListItemResponse)
  let details;
  try {
    details = await OfferService.getOfferById(id);
  } catch {
    details = offer;
  }

  document.getElementById("editOfferId").value           = id;
  document.getElementById("editOfferVehicleName").textContent =
    `${offer.brand} ${offer.model}`;
  document.getElementById("editOfferTitle").value        = details.title ?? "";
  document.getElementById("editOfferDescription").value  = details.description ?? "";
  document.getElementById("editOfferPrice").value        = details.price ?? "";
  document.getElementById("editOfferActive").checked     =
    // OfferDetailsResponse nie ma IsActive wprost — domyślnie true dla istniejących ofert
    details.isActive !== undefined ? details.isActive : true;

  clearError("formEditOfferError");
  openModal("modalEditOffer");
}

function _bindEditForm() {
  document.getElementById("formEditOffer").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearError("formEditOfferError");
    const btn = document.getElementById("btnEditOfferSubmit");
    setLoading(btn, true);

    const id = document.getElementById("editOfferId").value;

    try {
      await OfferService.updateOffer(id, {
        Title:          document.getElementById("editOfferTitle").value.trim(),
        Description:    document.getElementById("editOfferDescription").value.trim() || null,
        IsActive:       document.getElementById("editOfferActive").checked,
        NewPricePerDay: parseFloat(document.getElementById("editOfferPrice").value) || null,
      });
      closeModal("modalEditOffer");
      await loadOffers();
    } catch (err) {
      showError("formEditOfferError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ─── Delete modal ────────────────────────────────────────────
function _openDeleteModal(id) {
  const offer = allOffers.find((o) => o.id === id);
  pendingDeleteId = id;
  document.getElementById("deleteOfferName").textContent =
    offer ? offer.title : `#${id}`;
  openModal("modalDeleteOffer");
}

function _bindDeleteConfirm() {
  document.getElementById("btnDeleteOfferConfirm").addEventListener("click", async () => {
    if (!pendingDeleteId) return;
    const btn = document.getElementById("btnDeleteOfferConfirm");
    setLoading(btn, true);

    try {
      await OfferService.deleteOffer(pendingDeleteId);
      closeModal("modalDeleteOffer");
      pendingDeleteId = null;
      await loadOffers();
    } catch (err) {
      showError("formDeleteOfferError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ─── Helpers ─────────────────────────────────────────────────

// Wypełnia <select> pojazdami które nie mają jeszcze oferty
async function _populateVehicleSelect(selectId) {
  const select = document.getElementById(selectId);
  select.innerHTML = `<option value="">Loading…</option>`;

  try {
    // Pobierz wszystkie pojazdy i wszystkie oferty — odfiltruj zajęte vehicleId
    const [vehicles, offers] = await Promise.all([
      CarService.getAllCars({ status: 1 }), // tylko Available
      OfferService.getAllOffers(),
    ]);

    const takenVehicleIds = new Set(
      offers.map((o) => o.vehicleId).filter(Boolean)
    );

    const free = vehicles.filter((v) => !takenVehicleIds.has(v.id));

    if (!free.length) {
      select.innerHTML = `<option value="">No available vehicles without an offer</option>`;
      return;
    }

    select.innerHTML =
      `<option value="">Select vehicle…</option>` +
      free.map((v) => `<option value="${v.id}">${v.brand} — ${v.licensePlate} (${v.year})</option>`).join("");
  } catch {
    select.innerHTML = `<option value="">Failed to load vehicles</option>`;
  }
}