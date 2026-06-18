import { AuthService }   from "../authService.js";
import { UserService }   from "../userService.js";
import { initInsurances } from "./insurances.js";

// Route guard — only Admins allowed
(function guardRoute() {
  const role = AuthService.getRole();
  if (role !== "Admin") {
    window.location.href = "index.html";
  }
})();

// -- Tab binding --------------------------------------------
function bindTabs() {
  const tabs   = document.querySelectorAll(".tab-btn");
  const panels = document.querySelectorAll(".tab-panel");

  tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
      tabs.forEach((t)   => t.classList.remove("active"));
      panels.forEach((p) => p.classList.remove("active"));
      tab.classList.add("active");
      const panel = document.getElementById(tab.dataset.panel);
      if (panel) panel.classList.add("active");
    });
  });
}

// -- Toast notification --------------------------------------
function showToast(message, type = "success") {
  const toast = document.getElementById("admin-toast");
  if (!toast) return;

  toast.textContent = message;
  toast.className = `admin-toast admin-toast--${type} admin-toast--visible`;

  clearTimeout(toast._hideTimeout);
  toast._hideTimeout = setTimeout(() => {
    toast.classList.remove("admin-toast--visible");
  }, 4000);
}

// -- Render --------------------------------------------------
function renderEmployees(employees, tbody) {
  const emptyEl = document.getElementById("adminTableEmpty");

  if (employees.length === 0) {
    tbody.innerHTML = "";
    if (emptyEl) emptyEl.style.display = "block";
    return;
  }

  if (emptyEl) emptyEl.style.display = "none";

  tbody.innerHTML = employees.map((emp) => `
    <tr data-id="${emp.id}">
      <td>${emp.email}</td>
      <td>${emp.firstName} ${emp.lastName}</td>
      <td>${emp.workplaceId ?? "N/A"}</td>
      <td><span class="status-badge status-pending">Pending</span></td>
      <td>
        <div class="td-actions">
          <button class="btn-icon btn-icon--approve btn-approve" data-id="${emp.id}" title="Approve">
            <ion-icon name="checkmark-outline"></ion-icon>
          </button>
          <button class="btn-icon btn-icon--delete btn-reject" data-id="${emp.id}" title="Reject">
            <ion-icon name="close-outline"></ion-icon>
          </button>
        </div>
      </td>
    </tr>
  `).join("");

  tbody.querySelectorAll(".btn-approve").forEach((btn) => {
    btn.addEventListener("click", () => handleStatusChange(btn.dataset.id, 1, tbody));
  });

  tbody.querySelectorAll(".btn-reject").forEach((btn) => {
    btn.addEventListener("click", () => handleStatusChange(btn.dataset.id, 3, tbody));
  });
}

// -- Status change ------------------------------------------
async function handleStatusChange(id, status, tbody) {
  const action = status === 1 ? "approved" : "rejected";

  const row = tbody.querySelector(`tr[data-id="${id}"]`);
  if (row) {
    row.querySelectorAll("button").forEach((b) => (b.disabled = true));
  }

  try {
    await UserService.changeEmployeeStatus(id, status);
    if (row) {
      row.classList.add("row-fade-out");
      row.addEventListener("animationend", () => {
        row.remove();
        if (tbody.querySelectorAll("tr").length === 0) {
          const emptyEl = document.getElementById("adminTableEmpty");
          if (emptyEl) emptyEl.style.display = "block";
        }
      }, { once: true });
    }
    showToast(`Employee ${action} successfully.`, "success");
  } catch (error) {
    if (row) {
      row.querySelectorAll("button").forEach((b) => (b.disabled = false));
    }
    showToast(`Failed to ${action.replace("d", "")} employee: ${error.message}`, "error");
  }
}

// -- Init ----------------------------------------------------
async function initAdminPanel() {
  bindTabs();
  initInsurances();

  const tbody = document.getElementById("pending-employees-list");
  if (!tbody) return;

  try {
    const employees = await UserService.getPendingEmployees();
    renderEmployees(employees, tbody);
  } catch (error) {
    tbody.innerHTML = `
      <tr>
        <td colspan="5">
          <div class="table-empty">
            <ion-icon name="alert-circle-outline"></ion-icon>
            <p>Failed to load requests: ${error.message}</p>
          </div>
        </td>
      </tr>`;
  }
}

initAdminPanel();