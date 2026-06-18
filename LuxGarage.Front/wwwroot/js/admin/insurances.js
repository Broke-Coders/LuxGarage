import { InsuranceService } from "../insuranceService.js";
import { openModal, closeModal, setLoading, showError, clearError } from "../dashboard/utils/modal.js";

let allInsurances   = [];
let pendingDeleteId = null;

export function initInsurances() {
  _bindAddModal();
  _bindEditForm();
  _bindDeleteConfirm();
  loadInsurances();
}

// ── Load & render ─────────────────────────────────────────────────────────────
export async function loadInsurances() {
  const tbody   = document.getElementById("insurances-list");
  const emptyEl = document.getElementById("insurancesTableEmpty");

  tbody.innerHTML = `
    <tr class="table-loading-row">
      <td colspan="5">
        <div class="table-spinner">
          <div class="spinner"></div><span>Loading insurances…</span>
        </div>
      </td>
    </tr>`;
  emptyEl.style.display = "none";

  try {
    allInsurances = await InsuranceService.getAll();
    _renderTable(allInsurances);
  } catch (e) {
    tbody.innerHTML = `
      <tr><td colspan="5" style="text-align:center;padding:3rem;color:#c0392b;">
        ${e.message}
      </td></tr>`;
  }
}

function _renderTable(insurances) {
  const tbody   = document.getElementById("insurances-list");
  const emptyEl = document.getElementById("insurancesTableEmpty");

  if (!insurances.length) {
    tbody.innerHTML = "";
    emptyEl.style.display = "block";
    return;
  }
  emptyEl.style.display = "none";

  tbody.innerHTML = insurances.map((ins) => `
    <tr data-id="${ins.id}">
      <td>#${ins.id}</td>
      <td>${ins.name}</td>
      <td class="td-price">${Number(ins.pricePerDay).toFixed(2)} PLN</td>
      <td>
        <span class="status-badge ${ins.isActive ? "status-available" : "status-outofservice"}">
          ${ins.isActive ? "Active" : "Inactive"}
        </span>
      </td>
      <td>
        <div class="td-actions">
          <button class="btn-icon btn-icon--edit btn-ins-edit" data-id="${ins.id}" title="Edit">
            <ion-icon name="create-outline"></ion-icon>
          </button>
          <button class="btn-icon btn-icon--delete btn-ins-delete" data-id="${ins.id}" title="Delete">
            <ion-icon name="trash-outline"></ion-icon>
          </button>
        </div>
      </td>
    </tr>
  `).join("");

  tbody.querySelectorAll(".btn-ins-edit").forEach((btn) =>
    btn.addEventListener("click", () => _openEditModal(parseInt(btn.dataset.id)))
  );
  tbody.querySelectorAll(".btn-ins-delete").forEach((btn) =>
    btn.addEventListener("click", () => _openDeleteModal(parseInt(btn.dataset.id)))
  );
}

// ── Add modal ─────────────────────────────────────────────────────────────────
function _bindAddModal() {
  document.getElementById("btnAddInsurance").addEventListener("click", () => {
    document.getElementById("formInsuranceAdd").reset();
    document.getElementById("ins-add-active").checked = true;
    clearError("formInsuranceAddError");
    openModal("modalInsuranceAdd");
  });

  document.getElementById("formInsuranceAdd").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearError("formInsuranceAddError");
    const btn = document.getElementById("btnInsuranceAddSubmit");
    setLoading(btn, true);

    try {
      await InsuranceService.create({
        name:        document.getElementById("ins-add-name").value.trim(),
        pricePerDay: parseFloat(document.getElementById("ins-add-price").value),
        isActive:    document.getElementById("ins-add-active").checked,
      });
      closeModal("modalInsuranceAdd");
      showInsuranceToast("Insurance added successfully.", "success");
      await loadInsurances();
    } catch (err) {
      showError("formInsuranceAddError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ── Edit modal ────────────────────────────────────────────────────────────────
function _openEditModal(id) {
  const ins = allInsurances.find((i) => i.id === id);
  if (!ins) return;

  document.getElementById("ins-edit-id").value         = ins.id;
  document.getElementById("ins-edit-name").value        = ins.name;
  document.getElementById("ins-edit-price").value       = ins.pricePerDay;
  document.getElementById("ins-edit-active").checked    = ins.isActive;

  clearError("formInsuranceEditError");
  openModal("modalInsuranceEdit");
}

function _bindEditForm() {
  document.getElementById("formInsuranceEdit").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearError("formInsuranceEditError");
    const btn = document.getElementById("btnInsuranceEditSubmit");
    setLoading(btn, true);

    const id = parseInt(document.getElementById("ins-edit-id").value);

    try {
      await InsuranceService.update(id, {
        name:        document.getElementById("ins-edit-name").value.trim(),
        pricePerDay: parseFloat(document.getElementById("ins-edit-price").value),
        isActive:    document.getElementById("ins-edit-active").checked,
      });
      closeModal("modalInsuranceEdit");
      showInsuranceToast("Insurance updated successfully.", "success");
      await loadInsurances();
    } catch (err) {
      showError("formInsuranceEditError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ── Delete modal ──────────────────────────────────────────────────────────────
function _openDeleteModal(id) {
  const ins = allInsurances.find((i) => i.id === id);
  pendingDeleteId = id;
  document.getElementById("ins-delete-name").textContent = ins ? ins.name : `#${id}`;
  clearError("formInsuranceDeleteError");
  openModal("modalInsuranceDelete");
}

function _bindDeleteConfirm() {
  document.getElementById("btnInsuranceDeleteConfirm").addEventListener("click", async () => {
    if (!pendingDeleteId) return;
    const btn = document.getElementById("btnInsuranceDeleteConfirm");
    setLoading(btn, true);

    try {
      await InsuranceService.delete(pendingDeleteId);
      closeModal("modalInsuranceDelete");
      pendingDeleteId = null;
      showInsuranceToast("Insurance deleted successfully.", "success");
      await loadInsurances();
    } catch (err) {
      // 409 Conflict — insurance in use, pokazujemy błąd w modalu bez zamykania
      showError("formInsuranceDeleteError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// ── Toast (osobny element dla zakładki insurances) ────────────────────────────
function showInsuranceToast(message, type = "success") {
  const toast = document.getElementById("insurance-toast");
  if (!toast) return;

  toast.textContent = message;
  toast.className = `admin-toast admin-toast--${type} admin-toast--visible`;

  clearTimeout(toast._hideTimeout);
  toast._hideTimeout = setTimeout(() => {
    toast.classList.remove("admin-toast--visible");
  }, 4000);
}