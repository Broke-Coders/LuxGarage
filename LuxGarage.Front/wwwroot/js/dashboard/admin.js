import { UserService } from "../userService.js";

export async function initAdminPanel() {
    const listElement = document.getElementById("pending-employees-list");
    if (!listElement) return;

    try {
        const employees = await UserService.getPendingEmployees();
        renderEmployees(employees, listElement);
    } catch (error) {
        console.error("Failed to load pending employees:", error);
        listElement.innerHTML = `<tr><td colspan="5" style="text-align: center; color: red;">Error: ${error.message}</td></tr>`;
    }
}

function renderEmployees(employees, container) {
    if (employees.length === 0) {
        container.innerHTML = `<tr><td colspan="5" style="text-align: center; padding: 2rem;">No pending requests.</td></tr>`;
        return;
    }

    container.innerHTML = employees.map(emp => `
        <tr>
            <td>${emp.email}</td>
            <td>${emp.firstName} ${emp.lastName}</td>
            <td>${emp.workplaceId || 'N/A'}</td>
            <td><span class="status-badge status-pending">Pending</span></td>
            <td>
                <div class="action-btns">
                    <button class="btn-approve" data-id="${emp.id}">Approve</button>
                    <button class="btn-reject" data-id="${emp.id}">Reject</button>
                </div>
            </td>
        </tr>
    `).join('');

    // Bind events
    container.querySelectorAll(".btn-approve").forEach(btn => {
        btn.addEventListener("click", () => handleStatusChange(btn.dataset.id, 2)); // 2: Approved
    });

    container.querySelectorAll(".btn-reject").forEach(btn => {
        btn.addEventListener("click", () => handleStatusChange(btn.dataset.id, 3)); // 3: Rejected
    });
}

async function handleStatusChange(id, status) {
    const action = status === 2 ? "approve" : "reject";
    if (!confirm(`Are you sure you want to ${action} this employee?`)) return;

    try {
        await UserService.changeEmployeeStatus(id, status);
        alert(`Employee ${action}d successfully.`);
        initAdminPanel(); // Refresh list
    } catch (error) {
        alert(`Failed to ${action} employee: ` + error.message);
    }
}
