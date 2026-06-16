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
   * Pobiera profil zalogowanego użytkownika
   * @returns {Promise<Object|null>}
   */
  async getMyProfile() {
    const token = AuthService.getToken();
    if (!token) return null;

    const payload = parseJwt(token);
    // ClaimTypes.NameIdentifier mapuje się zazwyczaj na "nameid" lub "sub" w JWT
    const userId = payload?.nameid || payload?.sub;

    if (!userId) return null;

    const response = await fetch(`${API_BASE_URL}/Customer/${userId}`, {
      headers: authHeaders(),
    });
    const result = await handleResponse(response);
    return result?.data ?? result;
  }
};
