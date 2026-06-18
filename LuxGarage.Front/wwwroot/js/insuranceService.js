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

export const InsuranceService = {
  async getAll() {
    const res = await fetch(`${API_BASE_URL}/Insurances`, {
      headers: authHeaders(),
    });
    return await handleResponse(res);
  },

  async create(data) {
    const res = await fetch(`${API_BASE_URL}/Insurances`, {
      method: "POST",
      headers: { ...authHeaders(), "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    return await handleResponse(res);
  },

  async update(id, data) {
    const res = await fetch(`${API_BASE_URL}/Insurances/${id}`, {
      method: "PUT",
      headers: { ...authHeaders(), "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    return await handleResponse(res);
  },

  async delete(id) {
    const res = await fetch(`${API_BASE_URL}/Insurances/${id}`, {
      method: "DELETE",
      headers: authHeaders(),
    });
    return await handleResponse(res);
  },
};