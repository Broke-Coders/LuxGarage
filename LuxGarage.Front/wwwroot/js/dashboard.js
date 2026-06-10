/**
 * @file dashboard.js
 * @description Handles view switching and UI interactions for the Concierge Dashboard.
 */

document.addEventListener("DOMContentLoaded", () => {
   const sidebarLinks = document.querySelectorAll(".sidebar-link");
   const views = document.querySelectorAll(".dashboard-view");

   /**
    * Switches the active view based on the clicked sidebar link.
    * @param {string} viewName - The name of the view to activate (e.g., 'overview', 'fleet').
    */
   function switchView(viewName) {
      // Update sidebar links
      sidebarLinks.forEach(link => {
         if (link.dataset.view === viewName) {
            link.classList.add("active");
         } else {
            link.classList.remove("active");
         }
      });

      // Update visibility of view sections
      views.forEach(view => {
         if (view.id === `view-${viewName}`) {
            view.classList.add("active");
         } else {
            view.classList.remove("active");
         }
      });

      console.log(`Switched to view: ${viewName}`);
   }

   // Attach click events to sidebar links
   sidebarLinks.forEach(link => {
      link.addEventListener("click", (e) => {
         e.preventDefault();
         const viewName = link.dataset.view;
         if (viewName) {
            switchView(viewName);
         }
      });
   });
});
