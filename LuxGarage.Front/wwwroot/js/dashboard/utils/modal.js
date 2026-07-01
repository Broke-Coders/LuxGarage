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

export function showToast(message, type = "success") {
  let container = document.getElementById("toast-container");
  if (!container) {
    container = document.createElement("div");
    container.id = "toast-container";
    container.style.position = "fixed";
    container.style.bottom = "2.4rem";
    container.style.right = "2.4rem";
    container.style.zIndex = "9999";
    container.style.display = "flex";
    container.style.flexDirection = "column";
    container.style.gap = "1rem";
    document.body.appendChild(container);
  }

  const toast = document.createElement("div");
  toast.className = `custom-toast custom-toast--${type}`;
  toast.style.display = "flex";
  toast.style.alignItems = "center";
  toast.style.gap = "1.2rem";
  toast.style.padding = "1.4rem 2rem";
  toast.style.borderRadius = "8px";
  toast.style.background = "#fff";
  toast.style.boxShadow = "0 10px 30px rgba(0,0,0,0.12)";
  toast.style.fontFamily = "'Montserrat', sans-serif";
  toast.style.fontSize = "1.4rem";
  toast.style.minWidth = "28rem";
  toast.style.maxWidth = "40rem";
  toast.style.borderLeft = "4px solid #47453e";
  toast.style.transform = "translateX(120%)";
  toast.style.transition = "transform 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275), opacity 0.3s";
  toast.style.opacity = "0";

  let iconName = "checkmark-circle-outline";
  let iconColor = "#2ecc71";
  let borderLeftColor = "#2ecc71";

  if (type === "error") {
    iconName = "alert-circle-outline";
    iconColor = "#e74c3c";
    borderLeftColor = "#e74c3c";
  } else if (type === "warning") {
    iconName = "warning-outline";
    iconColor = "#f39c12";
    borderLeftColor = "#f39c12";
  } else if (type === "info") {
    iconName = "information-circle-outline";
    iconColor = "#3498db";
    borderLeftColor = "#3498db";
  }

  toast.style.borderLeftColor = borderLeftColor;

  toast.innerHTML = `
    <ion-icon name="${iconName}" style="font-size: 2.2rem; color: ${iconColor}; flex-shrink: 0;"></ion-icon>
    <div style="color: #47453e; font-weight: 500; flex-grow: 1; line-height: 1.4;">${message}</div>
    <button style="background: none; border: none; cursor: pointer; color: #8d8a7c; font-size: 1.8rem; display: flex; align-items: center; padding: 0;" onclick="this.parentElement.remove()">
      <ion-icon name="close-outline"></ion-icon>
    </button>
  `;

  container.appendChild(toast);

  setTimeout(() => {
    toast.style.transform = "translateX(0)";
    toast.style.opacity = "1";
  }, 10);

  setTimeout(() => {
    toast.style.transform = "translateX(120%)";
    toast.style.opacity = "0";
    setTimeout(() => {
      toast.remove();
      if (container.children.length === 0) {
        container.remove();
      }
    }, 300);
  }, 4000);
}