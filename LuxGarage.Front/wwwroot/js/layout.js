document.addEventListener("DOMContentLoaded", async () => {
    async function loadComponent(placeholderId, url) {
        try {
            const response = await fetch(url);
            if (!response.ok) throw new Error(`Failed to load ${url}`);
            
            const html = await response.text();
            document.getElementById(placeholderId).innerHTML = html;
        } catch (error) {
            console.error("Error loading component:", error);
        }
    }

    await loadComponent("navbar-placeholder", "./components/navbar.html");
    await loadComponent("footer-placeholder", "./components/footer.html");

    // TODO: function that check token and displays different option on the navbar
    // checkUserRoleAndModifyNavbar();
});