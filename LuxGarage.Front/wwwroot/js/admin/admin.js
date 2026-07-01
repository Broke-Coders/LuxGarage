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
function getStatusBadge(status) {
  switch (status) {
    case "Pending":
      return `<span class="status-badge status-pending">Pending</span>`;
    case "Approved":
      return `<span class="status-badge status-active">Approved</span>`;
    case "Rejected":
      return `<span class="status-badge status-cancelled">Rejected</span>`;
    case "Suspended":
      return `<span class="status-badge status-outofservice">Suspended</span>`;
    default:
      return `<span class="status-badge status-retired">${status || "Unknown"}</span>`;
  }
}

function getActionsHtml(emp) {
  const status = emp.status;
  if (status === "Pending") {
    return `
      <button class="btn-icon btn-icon--approve btn-status-change" data-id="${emp.id}" data-status="2" title="Approve">
        <ion-icon name="checkmark-outline"></ion-icon>
      </button>
      <button class="btn-icon btn-icon--delete btn-status-change" data-id="${emp.id}" data-status="3" title="Reject">
        <ion-icon name="close-outline"></ion-icon>
      </button>
    `;
  } else if (status === "Approved") {
    return `
      <button class="btn-icon btn-icon--delete btn-status-change" data-id="${emp.id}" data-status="4" title="Suspend Account">
        <ion-icon name="ban-outline"></ion-icon>
      </button>
    `;
  } else if (status === "Suspended") {
    return `
      <button class="btn-icon btn-icon--approve btn-status-change" data-id="${emp.id}" data-status="2" title="Activate Account">
        <ion-icon name="checkmark-outline"></ion-icon>
      </button>
    `;
  } else if (status === "Rejected") {
    return `
      <button class="btn-icon btn-icon--approve btn-status-change" data-id="${emp.id}" data-status="2" title="Approve">
        <ion-icon name="checkmark-outline"></ion-icon>
      </button>
    `;
  }
  return "";
}

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
      <td>${emp.workplaceName ?? "N/A"}</td>
      <td>${getStatusBadge(emp.status)}</td>
      <td>
        <div class="td-actions">
          ${getActionsHtml(emp)}
        </div>
      </td>
    </tr>
  `).join("");

  tbody.querySelectorAll(".btn-status-change").forEach((btn) => {
    btn.addEventListener("click", () => handleStatusChange(btn.dataset.id, parseInt(btn.dataset.status)));
  });
}

// -- Status change ------------------------------------------
async function handleStatusChange(id, status) {
  const statusLabels = {
    2: "Approved",
    3: "Rejected",
    4: "Suspended"
  };
  const actionLabel = statusLabels[status] || "Updated";

  try {
    await UserService.changeEmployeeStatus(id, status);
    showToast(`Employee status changed to ${actionLabel}.`, "success");
    await loadEmployees();
  } catch (error) {
    showToast(`Failed to update employee status: ${error.message}`, "error");
  }
}

// -- Load employees list ------------------------------------
async function loadEmployees() {
  const tbody = document.getElementById("employees-list");
  if (!tbody) return;

  try {
    const employees = await UserService.getAllEmployees();
    renderEmployees(employees, tbody);
  } catch (error) {
    tbody.innerHTML = `
      <tr>
        <td colspan="5">
          <div class="table-empty">
            <ion-icon name="alert-circle-outline"></ion-icon>
            <p>Failed to load employees: ${error.message}</p>
          </div>
        </td>
      </tr>`;
  }
}

// -- Init ----------------------------------------------------
async function initAdminPanel() {
  bindTabs();
  initInsurances();
  await loadEmployees();
}

initAdminPanel();