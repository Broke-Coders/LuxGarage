import { CarService }                        from "../carService.js";
import { openModal, closeModal, setLoading,
         showError, clearError }             from "./utils/modal.js";
import { statusBadge, statusValueFromLabel } from "./utils/statusHelpers.js";

let allVehicles   = [];
let addImageFiles = [];
let pendingDeleteId = null;

export function initVehicles() {
  _bindFilters();
  _bindAddModal();
  _bindEditForm();
  _bindDeleteConfirm();
  loadVehicles();
}

// -- Load and render -------------------------------
export async function loadVehicles(params = {}) {
  const tableBody  = document.getElementById("vehiclesTableBody");
  const tableEmpty = document.getElementById("tableEmpty");

  tableBody.innerHTML = `
    <tr class="table-loading-row">
      <td colspan="9">
        <div class="table-spinner">
          <div class="spinner"></div><span>Loading vehicles…</span>
        </div>
      </td>
    </tr>`;
  tableEmpty.style.display = "none";

  try {
    allVehicles = await CarService.getAllCars(params);
    _renderTable(allVehicles);
  } catch (e) {
    tableBody.innerHTML = `
      <tr><td colspan="9" style="text-align:center;padding:3rem;color:#c0392b;">
        ${e.message}
      </td></tr>`;
  }
}

function _renderTable(vehicles) {
  const tableBody  = document.getElementById("vehiclesTableBody");
  const tableEmpty = document.getElementById("tableEmpty");

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
          <button class="btn-icon btn-icon--gallery" data-id="${v.id}" title="Manage images">
            <ion-icon name="images-outline"></ion-icon>
          </button>
        </td>
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
    btn.addEventListener("click", () => _openEditModal(parseInt(btn.dataset.id)))
  );
  tableBody.querySelectorAll(".btn-icon--delete").forEach((btn) =>
    btn.addEventListener("click", () => _openDeleteModal(parseInt(btn.dataset.id)))
  );
}

// -- Filters ------------------
function _bindFilters() {
  const filterSearch = document.getElementById("filterSearch");
  const filterStatus = document.getElementById("filterStatus");
  const filterBody   = document.getElementById("filterBodyType");
  const btnReset     = document.getElementById("btnResetFilters");

  const getParams = () => ({
    searchTerm: filterSearch.value.trim(),
    status:     filterStatus.value,
    bodyType:   filterBody.value,
  });

  let debounce;
  filterSearch.addEventListener("input", () => {
    clearTimeout(debounce);
    debounce = setTimeout(() => loadVehicles(getParams()), 350);
  });
  filterStatus.addEventListener("change", () => loadVehicles(getParams()));
  filterBody.addEventListener("change",   () => loadVehicles(getParams()));
  btnReset.addEventListener("click", () => {
    filterSearch.value = "";
    filterStatus.value = "";
    filterBody.value   = "";
    loadVehicles();
  });
}

// -- Add modal -----------------------------
function _bindAddModal() {
  const dropZone    = document.getElementById("addImageDropZone");
  const fileInput   = document.getElementById("add-images");
  const previewList = document.getElementById("addImagePreview");

  document.getElementById("btnAddVehicle").addEventListener("click", () => {
    document.getElementById("formAdd").reset();
    addImageFiles = [];
    previewList.innerHTML = "";
    clearError("formAddError");
    openModal("modalAdd");
  });

  dropZone.addEventListener("click", () => fileInput.click());
  dropZone.addEventListener("dragover",  (e) => { e.preventDefault(); dropZone.classList.add("drag-over"); });
  dropZone.addEventListener("dragleave", () => dropZone.classList.remove("drag-over"));
  dropZone.addEventListener("drop", (e) => {
    e.preventDefault();
    dropZone.classList.remove("drag-over");
    _addFilesToQueue([...e.dataTransfer.files], previewList);
  });
  fileInput.addEventListener("change", () => {
    _addFilesToQueue([...fileInput.files], previewList);
    fileInput.value = "";
  });

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
      addImageFiles.filter(Boolean).forEach((f) => formData.append("Images", f));

      await CarService.createCar(formData);
      closeModal("modalAdd");
      await loadVehicles();
    } catch (err) {
      showError("formAddError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

function _addFilesToQueue(files, previewList) {
  files.forEach((file) => {
    if (!file.type.startsWith("image/")) return;
    const idx = addImageFiles.push(file) - 1;
    const reader = new FileReader();
    reader.onload = (e) => {
      const item = document.createElement("div");
      item.className = "preview-item";
      item.innerHTML = `
        <img src="${e.target.result}" alt="${file.name}" />
        <button class="preview-remove" title="Remove">×</button>`;
      item.querySelector(".preview-remove").addEventListener("click", () => {
        addImageFiles[idx] = null;
        item.remove();
      });
      previewList.appendChild(item);
    };
    reader.readAsDataURL(file);
  });
}

// -- Edit modal -----------------------------------
function _openEditModal(id) {
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

function _bindEditForm() {
  document.getElementById("formEdit").addEventListener("submit", async (e) => {
    e.preventDefault();
    clearError("formEditError");
    const btn = document.getElementById("btnEditSubmit");
    setLoading(btn, true);

    const id      = document.getElementById("edit-id").value;
    const mileage = parseInt(document.getElementById("edit-mileage").value);
    const status  = parseInt(document.getElementById("edit-status").value);

    try {
      await CarService.updateCar(id, { Mileage: mileage, Status: status });
      closeModal("modalEdit");
      await loadVehicles();
    } catch (err) {
      showError("formEditError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// -- Delete modal -----------------------------
function _openDeleteModal(id) {
  const vehicle = allVehicles.find((v) => v.id === id);
  pendingDeleteId = id;
  document.getElementById("delete-vehicle-name").textContent =
    vehicle ? `${vehicle.brand} (${vehicle.licensePlate})` : `#${id}`;
  openModal("modalDelete");
}

function _bindDeleteConfirm() {
  document.getElementById("btnDeleteConfirm").addEventListener("click", async () => {
    if (!pendingDeleteId) return;
    const btn = document.getElementById("btnDeleteConfirm");
    setLoading(btn, true);

    try {
      await CarService.deleteCar(pendingDeleteId);
      closeModal("modalDelete");
      pendingDeleteId = null;
      await loadVehicles();
    } catch (err) {
      showError("formDeleteError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}