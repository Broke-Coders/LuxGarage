import { AuthService } from "./authService.js";

const API_BASE_URL = "http://localhost:5054/api";

function authHeaders() {
  return { Authorization: `Bearer ${AuthService.getToken()}` };
}

async function handleResponse(response) {
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.message || errorData.Message || "API Request failed");
  }
  return response.json();
}

export const PaymentService = {
  /**
   * Processes a payment for a specific rental
   * @param {number} rentalId 
   * @param {string} paymentMethod 
   * @returns {Promise<Object>}
   */
  async processPayment(rentalId, paymentMethod = "MockCard") {
    const response = await fetch(`${API_BASE_URL}/Payment/process`, {
      method: "POST",
      headers: {
        ...authHeaders(),
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ rentalId, paymentMethod })
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  }
};
