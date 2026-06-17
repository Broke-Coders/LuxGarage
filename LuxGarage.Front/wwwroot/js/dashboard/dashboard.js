import { AuthService }    from "../authService.js";
import { bindModalClose } from "./utils/modal.js";
import { initVehicles }   from "./vehicles.js";
import { initOffers }     from "./offers.js";
import { initAdminPanel } from "./admin.js";

(function guardRoute() {
  const role = AuthService.getRole();
  if (role !== "Employee" && role !== "Admin") {
    window.location.href = "index.html";
  }
})();

async function loadComponent(placeholderId, url) {
  const res  = await fetch(url);
  const html = await res.text();
  const element = document.getElementById(placeholderId);
  if (element) {
    element.innerHTML = html;
  }
}

async function init() {
  const role = AuthService.getRole();
  const isAdmin = role === "Admin";

  const loadTasks = [
    loadComponent("panel-vehicles",    "../components/dashboard/panel-vehicles.html"),
    loadComponent("panel-offers",      "../components/dashboard/panel-offers.html"),
    loadComponent("panel-rentals",     "../components/dashboard/panel-rentals.html"),
    loadComponent("modals-placeholder","../components/dashboard/modals-vehicles.html"),
  ];

  if (isAdmin) {
    loadTasks.push(loadComponent("panel-admin", "../components/dashboard/panel-admin.html"));
    const adminTab = document.getElementById("tab-admin");
    if (adminTab) adminTab.style.display = "block";
  }

  // Load all panels
  await Promise.all(loadTasks);

    // Doładuj modale ofert do tego samego placeholdera
  const extra = await fetch("./components/dashboard/modals-offers.html");
  const modalsPlaceholder = document.getElementById("modals-placeholder");
  if (modalsPlaceholder) {
    modalsPlaceholder.innerHTML += await extra.text();
  }

  // Bind tabs and modals
  _bindTabs();
  bindModalClose();
  initVehicles();
  initOffers();
  
  if (isAdmin) {
    initAdminPanel();
  }
}

function _bindTabs() {
  const tabs   = document.querySelectorAll(".tab-btn");
  const panels = document.querySelectorAll(".tab-panel");

  tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
      tabs.forEach((t)   => t.classList.remove("active"));
      panels.forEach((p) => p.classList.remove("active"));
      tab.classList.add("active");
      const panel = document.getElementById(tab.dataset.panel);
      if (panel) {
        panel.classList.add("active");
      }
    });
  });
}

init();
