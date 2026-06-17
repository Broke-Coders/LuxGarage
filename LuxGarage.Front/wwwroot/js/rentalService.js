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

export const RentalService = {
  /**
   * POST /api/rental — tworzy nową rezerwację
   * @param {Object} data - Dane rezerwacji (CreateRentalRequest)
   * @returns {Promise<Object>}
   */
  async createRental(data) {
    const response = await fetch(`${API_BASE_URL}/rental`, {
      method: "POST",
      headers: { 
        ...authHeaders(), 
        "Content-Type": "application/json" 
      },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  /**
   * GET /api/rental/my-rentals — pobiera rezerwacje zalogowanego użytkownika
   * @returns {Promise<Array>}
   */
  async getMyRentals() {
    const response = await fetch(`${API_BASE_URL}/rental/my-rentals`, {
      headers: authHeaders(),
    });
    return handleResponse(response);
  },

  /**
   * GET /api/rental/vehicle/{vehicleId}/unavailable-dates — gets unavailable dates
   * @param {number} vehicleId
   * @returns {Promise<Array>}
   */
  async getUnavailableDates(vehicleId) {
    const response = await fetch(`${API_BASE_URL}/rental/vehicle/${vehicleId}/unavailable-dates`, {
      headers: authHeaders(),
    });
    return handleResponse(response);
  }
};
