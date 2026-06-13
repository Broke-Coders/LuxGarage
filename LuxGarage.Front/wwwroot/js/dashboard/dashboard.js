import { AuthService } from "./authService.js";
import { CarService } from "./carService.js";

// ─── Route guard ─────────────────────────────────────────────
(function guardRoute() {
  const role = AuthService.getRole();
  if (role !== "Employee" && role !== "Admin") {
    window.location.href = "index.html";
  }
})();

// ─── Status helpers ──────────────────────────────────────────
const STATUS_MAP = {
  1: { label: "Available",      css: "available" },
  2: { label: "Rented",         css: "rented" },
  3: { label: "Maintenance",    css: "maintenance" },
  4: { label: "Out of Service", css: "outofservice" },
  5: { label: "Retired",        css: "retired" },
};

function statusBadge(statusStr) {
  const normalized = statusStr?.toLowerCase().replace(/\s+/g, "");
  const entry = Object.values(STATUS_MAP).find(
    (s) => s.css === normalized
  );
  if (!entry) return `<span class="status-badge">${statusStr ?? "—"}</span>`;
  return `<span class="status-badge status-${entry.css}">${entry.label}</span>`;
}

function statusValueFromLabel(label) {
  const entry = Object.entries(STATUS_MAP).find(
    ([, v]) => v.label.toLowerCase() === label?.toLowerCase()
  );
  return entry ? parseInt(entry[0]) : 1;
}

// ─── State ───────────────────────────────────────────────────
let allVehicles    = [];
let addImageFiles  = [];
let pendingDeleteId = null;

// ─── DOM refs ────────────────────────────────────────────────
const tableBody     = document.getElementById("vehiclesTableBody");
const tableEmpty    = document.getElementById("tableEmpty");
const filterSearch  = document.getElementById("filterSearch");
const filterStatus  = document.getElementById("filterStatus");
const filterBody    = document.getElementById("filterBodyType");
const btnReset      = document.getElementById("btnResetFilters");
const btnAddVehicle = document.getElementById("btnAddVehicle");

// ─── Load vehicles ───────────────────────────────────────────
async function loadVehicles(params = {}) {
  tableBody.innerHTML = `
    <tr class="table-loading-row">
      <td colspan="8">
        <div class="table-spinner">
          <div class="spinner"></div><span>Loading vehicles…</span>
        </div>
      </td>
    </tr>`;
  tableEmpty.style.display = "none";

  try {
    allVehicles = await CarService.getAllCars(params);
    renderTable(allVehicles);
  } catch (e) {
    tableBody.innerHTML = `
      <tr><td colspan="8" style="text-align:center;padding:3rem;color:#c0392b;">
        ${e.message}
      </td></tr>`;
  }
}

function renderTable(vehicles) {
  if (!vehicles.length) {
    tableBody.innerHTML = "";
    tableEmpty.style.display = "block";
    return;
  }
  tableEmpty.style.display = "none";

  tableBody.innerHTML = vehicles
    .map(
      (v) => `
      <tr>
        <td>#${v.id}</td>
        <td class="td-brand-model"><strong>${v.brand}</strong></td>
        <td>${v.licensePlate}</td>
        <td>${v.year}</td>
        <td>${v.mileage.toLocaleString()} km</td>
        <td>${v.horsepower} HP</td>
        <td>${statusBadge(v.status)}</td>
        <td>
          <div class="td-actions">
            <button class="btn-icon btn-icon--edit"   data-id="${v.id}" title="Edit">
              <ion-icon name="create-outline"></ion-icon>
            </button>
            <button class="btn-icon btn-icon--delete" data-id="${v.id}" title="Delete">
              <ion-icon name="trash-outline"></ion-icon>
            </button>
          </div>
        </td>
      </tr>`
    )
    .join("");

  tableBody.querySelectorAll(".btn-icon--edit").forEach((btn) =>
    btn.addEventListener("click", () => openEditModal(parseInt(btn.dataset.id)))
  );
  tableBody.querySelectorAll(".btn-icon--delete").forEach((btn) =>
    btn.addEventListener("click", () => openDeleteModal(parseInt(btn.dataset.id)))
  );
}

// ─── Filters ────────────────────────────────────────────────
function getFilterParams() {
  return {
    searchTerm: filterSearch.value.trim(),
    status:     filterStatus.value,
    bodyType:   filterBody.value,
  };
}

let filterDebounce;
filterSearch.addEventListener("input", () => {
  clearTimeout(filterDebounce);
  filterDebounce = setTimeout(() => loadVehicles(getFilterParams()), 350);
});
filterStatus.addEventListener("change", () => loadVehicles(getFilterParams()));
filterBody.addEventListener("change",   () => loadVehicles(getFilterParams()));
btnReset.addEventListener("click", () => {
  filterSearch.value = "";
  filterStatus.value = "";
  filterBody.value   = "";
  loadVehicles();
});

// ─── Modal helpers ───────────────────────────────────────────
function openModal(id)  { document.getElementById(id).classList.add("active"); }
function closeModal(id) { document.getElementById(id).classList.remove("active"); }

document.querySelectorAll("[data-modal]").forEach((el) =>
  el.addEventListener("click", () => closeModal(el.dataset.modal))
);
document.querySelectorAll(".modal-overlay").forEach((overlay) =>
  overlay.addEventListener("click", (e) => {
    if (e.target === overlay) closeModal(overlay.id);
  })
);

function setLoading(btnEl, loading) {
  btnEl.disabled = loading;
  btnEl.querySelector(".btn-text").style.display    = loading ? "none" : "";
  btnEl.querySelector(".btn-spinner").style.display = loading ? "inline-flex" : "none";
}

function showError(elId, msg) {
  const el = document.getElementById(elId);
  el.textContent = msg;
  el.style.display = "block";
}

function clearError(elId) {
  const el = document.getElementById(elId);
  el.textContent = "";
  el.style.display = "none";
}

// ─── ADD MODAL ───────────────────────────────────────────────
btnAddVehicle.addEventListener("click", () => {
  document.getElementById("formAdd").reset();
  addImageFiles = [];
  document.getElementById("addImagePreview").innerHTML = "";
  clearError("formAddError");
  openModal("modalAdd");
});

// Image drop zone
const dropZone    = document.getElementById("addImageDropZone");
const fileInput   = document.getElementById("add-images");
const previewList = document.getElementById("addImagePreview");

dropZone.addEventListener("click", () => fileInput.click());
dropZone.addEventListener("dragover",  (e) => { e.preventDefault(); dropZone.classList.add("drag-over"); });
dropZone.addEventListener("dragleave", () => dropZone.classList.remove("drag-over"));
dropZone.addEventListener("drop", (e) => {
  e.preventDefault();
  dropZone.classList.remove("drag-over");
  addFilesToQueue([...e.dataTransfer.files]);
});
fileInput.addEventListener("change", () => {
  addFilesToQueue([...fileInput.files]);
  fileInput.value = "";
});

function addFilesToQueue(files) {
  files.forEach((file) => {
    if (!file.type.startsWith("image/")) return;
    const idx = addImageFiles.push(file) - 1;
    const reader = new FileReader();
    reader.onload = (e) => {
      const item = document.createElement("div");
      item.className = "preview-item";
      item.dataset.idx = idx;
      item.innerHTML = `
        <img src="${e.target.result}" alt="${file.name}" />
        <button class="preview-remove" title="Remove">×</button>`;
      item.querySelector(".preview-remove").addEventListener("click", () => {
        addImageFiles[idx] = null; // nullify so other indices stay valid
        item.remove();
      });
      previewList.appendChild(item);
    };
    reader.readAsDataURL(file);
  });
}

document.getElementById("formAdd").addEventListener("submit", async (e) => {
  e.preventDefault();
  clearError("formAddError");

  const btn = document.getElementById("btnAddSubmit");
  setLoading(btn, true);

  try {
    const form = e.target;
    const formData = new FormData();
    formData.append("Brand",        form.querySelector("#add-brand").value.trim());
    formData.append("Model",        form.querySelector("#add-model").value.trim());
    formData.append("LicensePlate", form.querySelector("#add-plate").value.trim());
    formData.append("EngineName",   form.querySelector("#add-engine").value.trim());
    formData.append("Year",         form.querySelector("#add-year").value);
    formData.append("Horsepower",   form.querySelector("#add-hp").value);
    formData.append("Mileage",      form.querySelector("#add-mileage").value);
    formData.append("ToHundred",    form.querySelector("#add-tohundred").value);
    formData.append("EngineType",   form.querySelector("#add-enginetype").value);
    formData.append("BodyType",     form.querySelector("#add-bodytype").value);
    formData.append("Color",        form.querySelector("#add-color").value);
    formData.append("Status",       form.querySelector("#add-status").value);

   const validFiles = addImageFiles.filter(Boolean);
   validFiles.forEach((f) => formData.append("Images", f));

   const created = await CarService.createCar(formData);

    closeModal("modalAdd");
    await loadVehicles(getFilterParams());
  } catch (err) {
    showError("formAddError", err.message);
  } finally {
    setLoading(btn, false);
  }
});

// ─── EDIT MODAL ──────────────────────────────────────────────
function openEditModal(id) {
  const vehicle = allVehicles.find((v) => v.id === id);
  if (!vehicle) return;

  document.getElementById("edit-id").value = id;
  document.getElementById("edit-vehicle-name").textContent =
    `${vehicle.brand} — ${vehicle.licensePlate}`;
  document.getElementById("edit-mileage").value = vehicle.mileage;
  document.getElementById("edit-status").value  = statusValueFromLabel(vehicle.status);

  clearError("formEditError");
  openModal("modalEdit");
}

document.getElementById("formEdit").addEventListener("submit", async (e) => {
  e.preventDefault();
  clearError("formEditError");

  const btn     = document.getElementById("btnEditSubmit");
  setLoading(btn, true);

  const id      = document.getElementById("edit-id").value;
  const mileage = parseInt(document.getElementById("edit-mileage").value);
  const status  = parseInt(document.getElementById("edit-status").value);

  try {
    await CarService.updateCar(id, { Mileage: mileage, Status: status });
    closeModal("modalEdit");
    await loadVehicles(getFilterParams());
  } catch (err) {
    showError("formEditError", err.message);
  } finally {
    setLoading(btn, false);
  }
});

// ─── DELETE MODAL ────────────────────────────────────────────
function openDeleteModal(id) {
  const vehicle = allVehicles.find((v) => v.id === id);
  pendingDeleteId = id;
  document.getElementById("delete-vehicle-name").textContent =
    vehicle ? `${vehicle.brand} (${vehicle.licensePlate})` : `#${id}`;
  openModal("modalDelete");
}

document.getElementById("btnDeleteConfirm").addEventListener("click", async () => {
  if (!pendingDeleteId) return;

  const btn = document.getElementById("btnDeleteConfirm");
  setLoading(btn, true);

  try {
    await CarService.deleteCar(pendingDeleteId);
    closeModal("modalDelete");
    pendingDeleteId = null;
    await loadVehicles(getFilterParams());
  } catch (err) {
    showError("formDeleteError", err.message);
  } finally {
    setLoading(btn, false);
  }
});

// ─── Init ────────────────────────────────────────────────────
loadVehicles();