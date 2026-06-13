import { AuthService }    from "../authService.js";
import { bindModalClose } from "./utils/modal.js";
import { initVehicles }   from "./vehicles.js";

(function guardRoute() {
  const role = AuthService.getRole();
  if (role !== "Employee" && role !== "Admin") {
    window.location.href = "index.html";
  }
})();

async function loadComponent(placeholderId, url) {
  const res  = await fetch(url);
  const html = await res.text();
  document.getElementById(placeholderId).innerHTML = html;
}

async function init() {
  // Load all panels
  await Promise.all([
    loadComponent("panel-vehicles",    "../components/dashboard/panel-vehicles.html"),
    loadComponent("panel-offers",      "../components/dashboard/panel-offers.html"),
    loadComponent("panel-rentals",     "../components/dashboard/panel-rentals.html"),
    loadComponent("modals-placeholder","../components/dashboard/modals-vehicles.html"),
  ]);

  // Bind tabs and modals
  _bindTabs();
  bindModalClose();
  initVehicles();
}

function _bindTabs() {
  const tabs   = document.querySelectorAll(".tab-btn");
  const panels = document.querySelectorAll(".tab-panel");

  tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
      tabs.forEach((t)   => t.classList.remove("active"));
      panels.forEach((p) => p.classList.remove("active"));
      tab.classList.add("active");
      document.getElementById(tab.dataset.panel).classList.add("active");
    });
  });
}

init();