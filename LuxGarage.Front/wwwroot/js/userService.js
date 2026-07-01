import { AuthService } from "./authService.js";

const API_BASE_URL = "http://localhost:5054/api";

function authHeaders() {
  const token = AuthService.getToken();
  return token ? { Authorization: `Bearer ${token}` } : {};
}

async function handleResponse(response) {
  if (response.status === 204) return null;
  const json = await response.json().catch(() => ({}));
  if (!response.ok) {
    throw new Error(json.Message || json.message || `Request failed (${response.status})`);
  }
  return json;
}

/**
 * Decodes JWT token to get payload.
 * @param {string} token 
 * @returns {Object|null}
 */
function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
}

export const UserService = {
  /**
   * Fetches the profile of the logged-in user
   * @returns {Promise<Object|null>}
   */
  async getMyProfile() {
    const token = AuthService.getToken();
    if (!token) return null;

    const payload = parseJwt(token);
    // ClaimTypes.NameIdentifier usually maps to "nameid" or "sub" in JWT
    const userId = payload?.nameid || payload?.sub;

    if (!userId) return null;

    const response = await fetch(`${API_BASE_URL}/Customer/${userId}`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  },

  /**
   * Updates the profile of the logged-in user
   * @param {Object} data 
   * @returns {Promise<Object>}
   */
  async updateProfile(data) {
    const token = AuthService.getToken();
    if (!token) throw new Error("Not logged in");

    const payload = parseJwt(token);
    const userId = payload?.nameid || payload?.sub;

    if (!userId) throw new Error("Invalid token");

    const response = await fetch(`${API_BASE_URL}/Customer/${userId}`, {
      method: "PUT",
      headers: { 
        ...authHeaders(), 
        "Content-Type": "application/json" 
      },
      body: JSON.stringify(data),
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  },

  /**
   * Fetches the list of employees waiting for approval (Admin only)
   * @returns {Promise<Array>}
   */
  async getPendingEmployees() {
    const response = await fetch(`${API_BASE_URL}/Employee/pending-employees`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? [];
  },

  /**
   * Changes the employee account status (Admin only)
   * @param {number} employeeId 
   * @param {number} status (1: Active, 2: Pending, 3: Disabled)
   * @returns {Promise<void>}
   */
  /**
   * Fetches the list of all employees (Admin only)
   * @returns {Promise<Array>}
   */
  async getAllEmployees() {
    const response = await fetch(`${API_BASE_URL}/Employee`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? [];
  },

  /**
   * Changes the employee account status (Admin only)
   * @param {number} employeeId 
   * @param {number} status (1: Pending, 2: Approved, 3: Rejected, 4: Suspended)
   * @returns {Promise<void>}
   */
  async changeEmployeeStatus(employeeId, status) {
    const response = await fetch(`${API_BASE_URL}/Employee/${employeeId}/status`, {
      method: "PUT",
      headers: {
        ...authHeaders(),
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ status })
    });
    return await handleResponse(response);
  }
};
