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
    const result = await handleResponse(response);
    return result?.data ?? result;
  },

  /**
   * GET /api/rental/my-rentals — pobiera rezerwacje zalogowanego użytkownika
   * @returns {Promise<Array>}
   */
  async getMyRentals() {
    const response = await fetch(`${API_BASE_URL}/rental/my-rentals`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result ?? [];
  },

  /**
   * GET /api/rental — pobiera wszystkie rezerwacje (dla admina/pracownika)
   * @returns {Promise<Array>}
   */
  async getAllRentals() {
    const response = await fetch(`${API_BASE_URL}/rental`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result ?? [];
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
    const result = await handleResponse(response);
    return result?.data ?? result ?? [];
  },

  /**
   * PUT /api/rental/{id}/cancel - cancels a pending reservation
   * @param {number} rentalId
   * @returns {Promise<Object>}
   */
  async cancelRental(rentalId) {
    const response = await fetch(`${API_BASE_URL}/rental/${rentalId}/cancel`, {
      method: "PUT",
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  },

  /**
   * PUT /api/rental/{id}/accept - accepts a pending reservation
   * @param {number} rentalId
   * @returns {Promise<Object>}
   */
  async acceptRental(rentalId) {
    const response = await fetch(`${API_BASE_URL}/rental/${rentalId}/accept`, {
      method: "PUT",
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  }
};
