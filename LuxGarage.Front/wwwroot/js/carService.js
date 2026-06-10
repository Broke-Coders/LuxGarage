const API_BASE_URL = 'http://localhost:5054/api';

/**
 * @namespace CarService
 * @description Get all cars and get car by id services. 
 * These functions will be used in the CarList and CarDetails components to fetch data from the API.
 */

export const CarService = {

    /**
     * Fetches all cars from the API and returns an array of car objects. Each car object contains properties like 
     * brandName, modelName, year, and horsepower.
     * @memberof CarService
     * @returns {Promise<Array>} - an array of car objects fetched from the API.
     * @throws {Error} - If the API request fails, an error is thrown.
     */
    async getAllCars() {
        try {
            const response = await fetch(`${API_BASE_URL}/Vehicles`);
            if (!response.ok) {
                throw new Error("Failed getting list of cars");
            }
            const dto = await response.json();
            return dto.data;
        } catch (error) {
            console.error("Error fetching cars:", error);
            throw error;
        }
    },

        /**
         * Fetches a single car by its ID from the API and returns a car object. The car object contains properties like
         * brandName, modelName, year, and horsepower.
         * @memberof CarService
         * @param {number} id - The ID of the car to fetch.
         * @return {Promise<Object>} - A Promise that resolves to a car object fetched from the API.
         * @throws {Error} - If the API request fails, an error is thrown.
         */
    async getCarById(id) {
        try {
            const response = await fetch(`${API_BASE_URL}/Vehicle/${id}`);
            if (!response.ok) {
                throw new Error(`Cannot find car with id ${id}`);
            }
            const dto = await response.json();
            return dto.data;
        } catch (error) {
            console.error("Error fetching car:", error);
            throw error;
        }
    },

    /**
     * Fetches cars filtered by one or more body types from the API.
     * Sends a GET request with query parameters for each body type ID.
     * @async
     * @memberof CarService
     * @param {Array<number>} bodyTypeIds - Array of vehicle body type IDs to filter by (e.g., [1, 2, 3])
     * @returns {Promise<Array>} - An array of car objects matching the provided body type IDs
     * @throws {Error} - If the API request fails, an error is thrown with a descriptive message
     * @example
     * const sedans = await CarService.getCarsByBodyTypes([1]);
     * const sedansAndHatchbacks = await CarService.getCarsByBodyTypes([1, 2]);
     */
    async getCarsByBodyTypes(bodyTypeIds) {
        try {
            const queryString = bodyTypeIds.map(id => `bodyTypeIds=${id}`).join('&');
            const response = await fetch(`${API_BASE_URL}/Vehicles?${queryString}`);
            if (!response.ok) {
                throw new Error("Failed to get cars by body types");
            }
            const dto = await response.json();
            return dto.data;
        } catch (error) {
            console.error("Error fetching cars by body types:", error);
            throw error;
        }
    }
}