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

/**
 * @namespace CarService
 * @description Service for all Vehicle API operations in LuxGarage.
 */
export const CarService = {

  /**
   * Fetches all cars with optional filters (public endpoint).
   * @param {Object} params - { searchTerm, bodyType, status, yearFrom, yearTo, sortBy, descending }
   * @returns {Promise<Array>} Array of VehicleResponse objects.
   */
  async getAllCars(params = {}) {
    try {
      const qs = new URLSearchParams();
      if (params.searchTerm) qs.set("SearchTerm", params.searchTerm);
      if (params.bodyType)   qs.set("BodyType",   params.bodyType);
      if (params.status)     qs.set("Status",     params.status);
      if (params.yearFrom)   qs.set("YearFrom",   params.yearFrom);
      if (params.yearTo)     qs.set("YearTo",     params.yearTo);
      if (params.sortBy)     qs.set("SortBy",     params.sortBy);
      if (params.descending) qs.set("Descending", params.descending);

      const response = await fetch(`${API_BASE_URL}/Vehicles?${qs.toString()}`);
      const json = await handleResponse(response);
      return json?.data ?? json ?? [];
    } catch (error) {
      console.error("Error fetching cars:", error);
      throw error;
    }
  },

  /**
   * Fetches a single car by ID (public endpoint).
   * @param {number} id
   * @returns {Promise<Object>} VehicleResponse object.
   */
  async getCarById(id) {
    try {
      const response = await fetch(`${API_BASE_URL}/Vehicles/${id}`);
      const json = await handleResponse(response);
      return json?.data ?? json;
    } catch (error) {
      console.error("Error fetching car:", error);
      throw error;
    }
  },

  /**
   * Fetches cars filtered by one or more body type IDs (public endpoint).
   * @param {number[]} bodyTypeIds
   * @returns {Promise<Array>}
   */
  async getCarsByBodyTypes(bodyTypeIds) {
    try {
      const qs = bodyTypeIds.map((id) => `BodyType=${id}`).join("&");
      const response = await fetch(`${API_BASE_URL}/Vehicles?${qs}`);
      const json = await handleResponse(response);
      return json?.data ?? json ?? [];
    } catch (error) {
      console.error("Error fetching cars by body types:", error);
      throw error;
    }
  },


  /**
   * Creates a new vehicle with optional images (multipart/form-data).
   * @param {FormData} formData - Must contain all CreateVehicleRequest fields + optional Images files.
   * @returns {Promise<Object>} Created VehicleResponse.
   */
  async createCar(formData) {
    try {
      const response = await fetch(`${API_BASE_URL}/Vehicles`, {
        method: "POST",
        headers: authHeaders(), // No Content-Type — browser sets multipart boundary
        body: formData,
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error creating car:", error);
      throw error;
    }
  },

  /**
   * Updates mileage and status of an existing vehicle.
   * Maps to PUT /api/Vehicles/{id} with UpdateVehicleRequest body.
   * @param {number} id
   * @param {{ Mileage: number, Status: number }} data
   * @returns {Promise<Object>} Updated VehicleResponse.
   */
  async updateCar(id, data) {
    try {
      const response = await fetch(`${API_BASE_URL}/Vehicles/${id}`, {
        method: "PUT",
        headers: { ...authHeaders(), "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error updating car:", error);
      throw error;
    }
  },

  /**
   * Deletes a vehicle by ID.
   * Note: backend must handle cascading delete of related Offers first.
   * @param {number} id
   * @returns {Promise<null>}
   */
  async deleteCar(id) {
    try {
      const response = await fetch(`${API_BASE_URL}/Vehicles/${id}`, {
        method: "DELETE",
        headers: authHeaders(),
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error deleting car:", error);
      throw error;
    }
  },

  // VEHICLE IMAGES 

  /**
   * Fetches all images for a given vehicle.
   * Maps to GET /api/VehicleImages?vehicleId={id}
   * @param {number} vehicleId
   * @returns {Promise<Array>} Array of VehicleImageResponse objects.
   */
  async getImagesByVehicleId(vehicleId) {
    try {
      const response = await fetch(
        `${API_BASE_URL}/VehicleImages?vehicleId=${vehicleId}`,
        { headers: authHeaders() }
      );
      return await handleResponse(response);
    } catch (error) {
      console.error("Error fetching vehicle images:", error);
      throw error;
    }
  },

  /**
   * Uploads images for an existing vehicle.
   * Maps to POST /api/VehicleImages/upload (multipart/form-data).
   * @param {number} vehicleId
   * @param {File[]} files
   * @param {number|null} primaryImageIndex
   * @returns {Promise<Array>} Array of created VehicleImageResponse objects.
   */
  async uploadImages(vehicleId, files, primaryImageIndex = null) {
    try {
      const formData = new FormData();
      formData.append("VehicleId", vehicleId);
      files.forEach((f) => formData.append("Images", f));
      if (primaryImageIndex !== null) {
        formData.append("PrimaryImageIndex", primaryImageIndex);
      }

      const response = await fetch(`${API_BASE_URL}/VehicleImages/upload`, {
        method: "POST",
        headers: authHeaders(),
        body: formData,
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error uploading images:", error);
      throw error;
    }
  },

  /**
   * Deletes a single vehicle image by its ID.
   * Maps to DELETE /api/VehicleImages/{imageId}
   * @param {number} imageId
   * @returns {Promise<null>}
   */
  async deleteImage(imageId) {
    try {
      const response = await fetch(`${API_BASE_URL}/VehicleImages/${imageId}`, {
        method: "DELETE",
        headers: authHeaders(),
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error deleting image:", error);
      throw error;
    }
  },


  async getPrimaryImage(vehicleId) {
    try {
      const response = await fetch(`${API_BASE_URL}/VehicleImages/${vehicleId}/primary`,
        {
          method: "GET",
          headers: {...authHeaders(), "Content-Type": "application/json" },
          body: JSON.stringify({ vehicleId }),
        }
      );
      return await handleResponse(response);
    } catch (error) {
      console.error("Error getting primary image:", error);
      throw error;
    }
  },

  /**
   * Sets a specific image as the primary image for a vehicle.
   * Maps to PATCH /api/VehicleImages/{imageId}/set-primary
   * @param {number} vehicleId
   * @param {number} imageId
   * @returns {Promise<null>}
   */
  async setPrimaryImage(vehicleId, imageId) {
    try {
      const response = await fetch(
        `${API_BASE_URL}/VehicleImages/${imageId}/primary`,
        {
          method: "POST",
          headers: { ...authHeaders(), "Content-Type": "application/json" },
          body: JSON.stringify({ vehicleId }),
        }
      );
      return await handleResponse(response);
    } catch (error) {
      console.error("Error setting primary image:", error);
      throw error;
    }
  },

  /**
   * Reorders images for a vehicle.
   * Maps to PUT /api/VehicleImages/reorder
   * @param {number} vehicleId
   * @param {number[]} orderedImageIds - Full ordered list of image IDs.
   * @returns {Promise<null>}
   */
  async reorderImages(vehicleId, orderedImageIds) {
    try {
      const response = await fetch(`${API_BASE_URL}/VehicleImages/reorder`, {
        method: "PUT",
        headers: { ...authHeaders(), "Content-Type": "application/json" },
        body: JSON.stringify({ vehicleId, orderedImageIds }),
      });
      return await handleResponse(response);
    } catch (error) {
      console.error("Error reordering images:", error);
      throw error;
    }
  },
};