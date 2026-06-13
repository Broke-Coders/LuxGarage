export function openModal(id) {
  document.getElementById(id).classList.add("active");
}

export function closeModal(id) {
  document.getElementById(id).classList.remove("active");
}

export function setLoading(btnEl, loading) {
  btnEl.disabled = loading;
  btnEl.querySelector(".btn-text").style.display    = loading ? "none" : "";
  btnEl.querySelector(".btn-spinner").style.display = loading ? "inline-flex" : "none";
}

export function showError(elId, msg) {
  const el = document.getElementById(elId);
  el.textContent = msg;
  el.style.display = "block";
}

export function clearError(elId) {
  const el = document.getElementById(elId);
  el.textContent = "";
  el.style.display = "none";
}

export function bindModalClose() {
  document.querySelectorAll("[data-modal]").forEach((el) =>
    el.addEventListener("click", () => closeModal(el.dataset.modal))
  );
  document.querySelectorAll(".modal-overlay").forEach((overlay) =>
    overlay.addEventListener("click", (e) => {
      if (e.target === overlay) closeModal(overlay.id);
    })
  );
}