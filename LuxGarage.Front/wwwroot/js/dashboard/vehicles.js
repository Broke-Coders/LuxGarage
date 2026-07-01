import { CarService }                        from "../carService.js";
import { openModal, closeModal, setLoading,
         showError, clearError, showConfirm, showToast } from "./utils/modal.js";
import { statusBadge, statusValueFromLabel } from "./utils/statusHelpers.js";

let allVehicles   = [];
let addImageFiles = [];
let pendingDeleteId = null;

export function initVehicles() {
  _bindFilters();
  _bindAddModal();
  _bindEditForm();
  _bindDeleteConfirm();
  _bindGallery();
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
  tableBody.querySelectorAll(".btn-icon--gallery").forEach((btn) =>
    btn.addEventListener("click", () => _openGalleryModal(parseInt(btn.dataset.id)))
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
      
      // Clean and validate inputs
      let toHundredRaw = form.querySelector("#add-tohundred").value.trim().replace(',', '.');
      let toHundredVal = parseFloat(toHundredRaw);
      if (isNaN(toHundredVal)) {
        throw new Error("Speed to 100 km/h must be a valid number.");
      }
      const roundedToHundred = (Math.round(toHundredVal * 10) / 10).toFixed(1);

      let horsepowerRaw = form.querySelector("#add-hp").value.trim().replace(',', '.');
      let horsepowerVal = parseFloat(horsepowerRaw);
      if (isNaN(horsepowerVal)) {
        throw new Error("Horsepower must be a valid number.");
      }
      const roundedHorsepower = Math.round(horsepowerVal);

      const formData = new FormData();
      formData.append("Brand",        form.querySelector("#add-brand").value.trim());
      formData.append("Model",        form.querySelector("#add-model").value.trim());
      formData.append("LicensePlate", form.querySelector("#add-plate").value.trim());
      formData.append("EngineName",   form.querySelector("#add-engine").value.trim());
      formData.append("Year",         form.querySelector("#add-year").value);
      formData.append("Horsepower",   roundedHorsepower);
      formData.append("Mileage",      form.querySelector("#add-mileage").value);
      formData.append("ToHundred",    roundedToHundred);
      formData.append("EngineType",   form.querySelector("#add-enginetype").value);
      formData.append("BodyType",     form.querySelector("#add-bodytype").value);
      formData.append("Color",        form.querySelector("#add-color").value);
      formData.append("Status",       form.querySelector("#add-status").value);
      addImageFiles.filter(Boolean).forEach((f) => formData.append("Images", f));

      await CarService.createCar(formData);
      closeModal("modalAdd");

      // Reset form and clear image queue on success
      form.reset();
      addImageFiles = [];
      const previewList = document.getElementById("addImagePreview");
      if (previewList) previewList.innerHTML = "";

      showToast("Vehicle added successfully!", "success");
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
      showToast("Vehicle updated successfully!", "success");
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
      showToast("Vehicle deleted successfully!", "success");
      await loadVehicles();
    } catch (err) {
      showError("formDeleteError", err.message);
    } finally {
      setLoading(btn, false);
    }
  });
}

// -- Gallery modal -----------------------------
const API_ORIGIN = "http://localhost:5054";

let galleryVehicleId = null;
let galleryImages = [];    
let galleryDirty = false; 
let galleryUploadFiles = [];

function _bindGallery() {
  const dropZone  = document.getElementById("galleryDropZone");
  const fileInput = document.getElementById("galleryFileInput");

  dropZone.addEventListener("click",     () => fileInput.click());
  dropZone.addEventListener("dragover",  (e) => { e.preventDefault(); dropZone.classList.add("drag-over"); });
  dropZone.addEventListener("dragleave", () => dropZone.classList.remove("drag-over"));
  dropZone.addEventListener("drop", (e) => {
    e.preventDefault();
    dropZone.classList.remove("drag-over");
    _handleGalleryUpload([...e.dataTransfer.files]);
  });
  fileInput.addEventListener("change", () => {
    _handleGalleryUpload([...fileInput.files]);
    fileInput.value = "";
  });

  document.getElementById("btnSaveOrder").addEventListener("click", _saveOrder);
}

async function _openGalleryModal(vehicleId) {
  const vehicle = allVehicles.find((v) => v.id === vehicleId);
  galleryVehicleId   = vehicleId;
  galleryDirty       = false;
  galleryUploadFiles = [];

  document.getElementById("galleryVehicleName").textContent =
    vehicle ? `${vehicle.brand} — ${vehicle.licensePlate}` : `#${vehicleId}`;
  document.getElementById("btnSaveOrder").style.display       = "none";
  document.getElementById("galleryReorderHint").style.display = "none";
  clearError("galleryUploadError");

  openModal("modalGallery");
  await _loadGalleryImages();
}

async function _loadGalleryImages() {
  const grid  = document.getElementById("galleryGrid");
  const empty = document.getElementById("galleryEmpty");

  grid.innerHTML = `<div class="gallery-loading"><div class="spinner"></div><span>Loading…</span></div>`;
  empty.style.display = "none";

  try {
    galleryImages = await CarService.getImagesByVehicleId(galleryVehicleId);
    _renderGalleryGrid();
  } catch (e) {
    grid.innerHTML = `<p style="color:#c0392b;padding:2rem;">${e.message}</p>`;
  }
}

function _renderGalleryGrid() {
  const grid  = document.getElementById("galleryGrid");
  const empty = document.getElementById("galleryEmpty");

  if (!galleryImages.length) {
    grid.innerHTML = "";
    empty.style.display = "flex";
    return;
  }
  empty.style.display = "none";

  grid.innerHTML = galleryImages
    .map((img, idx) => `
      <div class="gallery-item" draggable="true" data-id="${img.id}" data-idx="${idx}">
        <div class="gallery-item-img-wrap">
          <img src="${API_ORIGIN}${img.url}" alt="Vehicle image ${idx + 1}" loading="lazy" />
          ${img.isPrimary ? `<span class="gallery-primary-badge"><ion-icon name="star"></ion-icon></span>` : ""}
        </div>
        <div class="gallery-item-actions">
          ${!img.isPrimary
            ? `<button class="gallery-btn gallery-btn--star" data-id="${img.id}" title="Set as primary">
                <ion-icon name="star-outline"></ion-icon>
               </button>`
            : `<span class="gallery-btn gallery-btn--star gallery-btn--star-active" title="Primary image">
                <ion-icon name="star"></ion-icon>
               </span>`
          }
          <button class="gallery-btn gallery-btn--delete" data-id="${img.id}" title="Delete image">
            <ion-icon name="trash-outline"></ion-icon>
          </button>
        </div>
      </div>`)
    .join("");

  // Set primary
  grid.querySelectorAll(".gallery-btn--star[data-id]").forEach((btn) =>
    btn.addEventListener("click", () => _setPrimary(parseInt(btn.dataset.id)))
  );

  // Delete
  grid.querySelectorAll(".gallery-btn--delete").forEach((btn) =>
    btn.addEventListener("click", () => _deleteImage(parseInt(btn.dataset.id)))
  );

  // Drag & drop reorder
  _initDragReorder(grid);
}

function _initDragReorder(grid) {
  let dragSrc = null;

  grid.querySelectorAll(".gallery-item").forEach((item) => {
    item.addEventListener("dragstart", (e) => {
      dragSrc = item;
      item.classList.add("dragging");
      e.dataTransfer.effectAllowed = "move";
    });

    item.addEventListener("dragend", () => {
      item.classList.remove("dragging");
      grid.querySelectorAll(".gallery-item").forEach((i) => i.classList.remove("drag-over-item"));
    });

    item.addEventListener("dragover", (e) => {
      e.preventDefault();
      e.dataTransfer.dropEffect = "move";
      if (item !== dragSrc) {
        grid.querySelectorAll(".gallery-item").forEach((i) => i.classList.remove("drag-over-item"));
        item.classList.add("drag-over-item");
      }
    });

    item.addEventListener("drop", (e) => {
      e.preventDefault();
      if (!dragSrc || dragSrc === item) return;

      // Przestaw w DOM
      const items   = [...grid.querySelectorAll(".gallery-item")];
      const srcIdx  = items.indexOf(dragSrc);
      const destIdx = items.indexOf(item);

      if (srcIdx < destIdx) {
        grid.insertBefore(dragSrc, item.nextSibling);
      } else {
        grid.insertBefore(dragSrc, item);
      }

      const newOrder = [...grid.querySelectorAll(".gallery-item")].map((el) =>
        galleryImages.find((img) => img.id === parseInt(el.dataset.id))
      );
      galleryImages = newOrder;

      galleryDirty = true;
      document.getElementById("btnSaveOrder").style.display       = "inline-flex";
      document.getElementById("galleryReorderHint").style.display = "flex";

      item.classList.remove("drag-over-item");
    });
  });
}

async function _setPrimary(imageId) {
  try {
    await CarService.setPrimaryImage(galleryVehicleId, imageId);
    showToast("Primary image updated!", "success");
    await _loadGalleryImages();
  } catch (e) {
    showError("galleryUploadError", e.message);
  }
}

async function _deleteImage(imageId) {
  const confirmed = await showConfirm("Delete Image", "Are you sure you want to delete this vehicle image?", true);
  if (!confirmed) return;
  try {
    await CarService.deleteImage(imageId);
    showToast("Image deleted successfully!", "success");
    await _loadGalleryImages();
  } catch (e) {
    showError("galleryUploadError", e.message);
  }
}

async function _saveOrder() {
  const btn = document.getElementById("btnSaveOrder");
  setLoading(btn, true);

  try {
    await CarService.reorderImages(galleryVehicleId, galleryImages.map((img) => img.id));
    galleryDirty = false;
    document.getElementById("btnSaveOrder").style.display       = "none";
    document.getElementById("galleryReorderHint").style.display = "none";
    showToast("Gallery order saved!", "success");
    await _loadGalleryImages();
  } catch (e) {
    showError("galleryUploadError", e.message);
  } finally {
    setLoading(btn, false);
  }
}

async function _handleGalleryUpload(files) {
  const validFiles = files.filter((f) => f.type.startsWith("image/"));
  if (!validFiles.length) return;

  clearError("galleryUploadError");
  const dropZone = document.getElementById("galleryDropZone");
  dropZone.classList.add("uploading");

  try {
    await CarService.uploadImages(galleryVehicleId, validFiles);
    showToast("Images uploaded successfully!", "success");
    await _loadGalleryImages();
  } catch (e) {
    showError("galleryUploadError", e.message);
  } finally {
    dropZone.classList.remove("uploading");
  }
}