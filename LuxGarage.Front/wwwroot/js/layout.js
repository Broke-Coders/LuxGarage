import { AuthService } from "./authService.js";

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

    async function validateToken() {
        const token = localStorage.getItem('jwtToken');
        if (!token) return;

        try {
            const response = await fetch('http://localhost:5054/api/Auth/validate', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                clearSession();
            }

        } catch (error) {
            console.warn('Token validation failed (network error):', error);
            clearSession(); 
        }
    }

    function clearSession() {
        localStorage.removeItem("jwtToken");
        localStorage.removeItem("userRole");
    }

    function buildUserMenu() {
        const dropdown = document.getElementById('userDropdown');
        const btn = document.getElementById('userIconBtn');
        if (!dropdown || !btn) return;

        const token = AuthService.getToken();
        const role = AuthService.getRole();

        let menuItems = '';

        if (!token) {
            menuItems = `<li><a href="login.html">Log in</a></li>`;
        } else {

            if (role === 'Customer') {
                menuItems += `<li><a href="account.html">My account</a></li>`;
            }
            if (role === 'Employee' || role === 'Admin') {
                menuItems += `<li><a href="dashboard.html">Dashboard</a></li>`;
            }
            if (role === 'Admin') {
                menuItems += `<li><a href="admin.html">Admin panel</a></li>`;
            }

            menuItems += `
                <li class="dropdown-divider"></li>
                <li><a href="#" id="logoutBtn">Log out</a></li>
            `;
        }

        dropdown.innerHTML = menuItems;

        document.getElementById('logoutBtn')?.addEventListener('click', (e) => {
            e.preventDefault();
            AuthService.logout();
        });

        // Toggle dropdown
        btn.addEventListener('click', (e) => {
            e.stopPropagation();
            const isOpen = dropdown.classList.toggle('open');
            btn.setAttribute('aria-expanded', isOpen);
        });

        // Close when clicked off menu
        document.addEventListener('click', () => {
            dropdown.classList.remove('open');
            btn.setAttribute('aria-expanded', 'false');
        });
    }

    await loadComponent("navbar-placeholder", "./components/navbar.html");
    await loadComponent("footer-placeholder", "./components/footer.html");

    await validateToken();
    buildUserMenu();
});