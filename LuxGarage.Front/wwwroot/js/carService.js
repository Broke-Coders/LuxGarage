const API_BASE_URL = 'http://localhost:5054/api';

/***
 * Get all cars and get car by id services. 
 * These functions will be used in the CarList and CarDetails components to fetch data from the API.
 */

export const CarService = {

    /***
     * Fetches all cars from the API and returns an array of car objects. Each car object contains properties like 
     * brandName, modelName, year, and horsepower.
     * @returns dto.data - an array of car objects fetched from the API.
     * @throws Error - If the API request fails, an error is thrown.
     */
    async getAllCars() {
        const response = await fetch(`${API_BASE_URL}/Vehicles`);
        if (!response.ok) {
            throw new Error("Failed getting list of cars");
        }
        const dto = await response.json();
        return dto.data;
    },

        /***
         * Fetches a single car by its ID from the API and returns a car object. The car object contains properties like
         * brandName, modelName, year, and horsepower.
         * @param {number} id - The ID of the car to fetch.
         * @return dto.data - A car object fetched from the API.
         * @throws Error - If the API request fails, an error is thrown.
         */
    async getCarById(id) {
        const response = await fetch(`${API_BASE_URL}/Vehicle/${id}`);
        if (!response.ok) {
            throw new Error("Cannot find car with id ${id}");
        }
        const dto = response.json();
        return dto.data;
    },
}