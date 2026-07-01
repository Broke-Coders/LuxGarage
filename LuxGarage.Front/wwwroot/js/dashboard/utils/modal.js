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

export function showConfirm(title, message, isDanger = true) {
  return new Promise((resolve) => {
    const modalId = `confirm-modal-${Date.now()}-${Math.floor(Math.random() * 1000)}`;

    const overlay = document.createElement("div");
    overlay.className = "modal-overlay active";
    overlay.id = modalId;

    const iconName = isDanger ? "warning-outline" : "help-circle-outline";
    const iconClass = isDanger ? "icon-warning" : "icon-info";
    const confirmButtonClass = isDanger ? "btn-danger" : "btn-submit";
    const confirmText = isDanger ? "Delete" : "Confirm";

    overlay.innerHTML = `
      <div class="modal-box modal-box--xs" style="animation: modal-in 0.22s ease;">
        <div class="modal-head">
          <h2>${title}</h2>
          <button class="modal-close" id="${modalId}-close-btn">
            <ion-icon name="close-outline"></ion-icon>
          </button>
        </div>
        <div class="modal-body-text">
          <ion-icon name="${iconName}" class="${iconClass}" style="${!isDanger ? 'font-size: 4.8rem; color: #3498db; margin-bottom: 1.2rem;' : ''}"></ion-icon>
          <p>${message}</p>
          ${isDanger ? '<p class="delete-note">This action cannot be undone.</p>' : ''}
        </div>
        <div class="modal-actions">
          <button type="button" class="btn-cancel" id="${modalId}-cancel-btn">Cancel</button>
          <button type="button" class="${confirmButtonClass}" id="${modalId}-confirm-btn">
            <span class="btn-text">${confirmText}</span>
          </button>
        </div>
      </div>
    `;

    document.body.appendChild(overlay);

    const cleanup = () => {
      overlay.classList.remove("active");
      setTimeout(() => {
        overlay.remove();
      }, 300);
    };

    const confirmBtn = document.getElementById(`${modalId}-confirm-btn`);
    const cancelBtn = document.getElementById(`${modalId}-cancel-btn`);
    const closeBtn = document.getElementById(`${modalId}-close-btn`);

    confirmBtn.addEventListener("click", () => {
      cleanup();
      resolve(true);
    });

    cancelBtn.addEventListener("click", () => {
      cleanup();
      resolve(false);
    });

    closeBtn.addEventListener("click", () => {
      cleanup();
      resolve(false);
    });

    overlay.addEventListener("click", (e) => {
      if (e.target === overlay) {
        cleanup();
        resolve(false);
      }
    });
  });
}