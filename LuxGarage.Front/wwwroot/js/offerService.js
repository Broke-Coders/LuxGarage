import { AuthService } from "./authService.js";

const API_BASE_URL = "http://localhost:5054/api";

function authHeaders() {
  return { Authorization: `Bearer ${AuthService.getToken()}` };
}

async function handleResponse(response) {
  if (response.status === 204) return null;
  const json = await response.json().catch(() => ({}));
  if (!response.ok) {
    throw new Error(json.Message || json.message || `Request failed (${response.status})`);
  }
  return json;
}

export const OfferService = {

  /**
   * GET /api/offers — lista ofert z opcjonalnym sortowaniem
   * @param {{ sortBy?: string, descending?: boolean }} params
   * @returns {Promise<OfferListItemResponse[]>}
   */
  async getAllOffers(params = {}) {
    const qs = new URLSearchParams();
    if (params.sortBy)     qs.set("SortBy",     params.sortBy);
    if (params.descending) qs.set("Descending", params.descending);

    const response = await fetch(`${API_BASE_URL}/offers?${qs.toString()}`);
    const json = await handleResponse(response);
    return json?.data ?? json ?? [];
  },

  /**
   * GET /api/offers/{id}
   * @param {number} id
   * @returns {Promise<OfferDetailsResponse>}
   */
  async getOfferById(id) {
    const response = await fetch(`${API_BASE_URL}/offers/${id}`);
    const json = await handleResponse(response);
    return json?.data ?? json;
  },

  /**
   * GET /api/offers/vehicle/{vehicleId}
   * @param {number} vehicleId
   * @returns {Promise<OfferDetailsResponse>}
   */
  async getOfferByVehicleId(vehicleId) {
    const response = await fetch(`${API_BASE_URL}/offers/vehicle/${vehicleId}`);
    const json = await handleResponse(response);
    return json?.data ?? json;
  },

  /**
   * POST /api/offers — tworzy nową ofertę (Employee/Admin)
   * @param {{ VehicleId: number, Title: string, Description?: string, InitialPricePerDay: number }} data
   * @returns {Promise<null>}
   */
  async createOffer(data) {
    const response = await fetch(`${API_BASE_URL}/offers`, {
      method: "POST",
      headers: { ...authHeaders(), "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  /**
   * PUT /api/offers/{id} — aktualizuje ofertę (Employee/Admin)
   * @param {number} id
   * @param {{ Title: string, Description?: string, IsActive: boolean, NewPricePerDay?: number }} data
   * @returns {Promise<null>}
   */
  async updateOffer(id, data) {
    const response = await fetch(`${API_BASE_URL}/offers/${id}`, {
      method: "PUT",
      headers: { ...authHeaders(), "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  /**
   * DELETE /api/offers/{id} — usuwa ofertę (Admin only)
   * @param {number} id
   * @returns {Promise<null>}
   */
  async deleteOffer(id) {
    const response = await fetch(`${API_BASE_URL}/offers/${id}`, {
      method: "DELETE",
      headers: authHeaders(),
    });
    return handleResponse(response);
  },
};