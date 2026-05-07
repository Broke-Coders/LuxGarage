const API_BASE_URL = 'http://localhost:5054/api';

export const CarService = {

    async getAllCars() {
        const response = await fetch(`${API_BASE_URL}/Vehicles`);
        if (!response.ok) {
            throw new Error("Failed getting list of cars");
        }
        const dto = await response.json();
        return dto.data;
    },

    async getCarById(id) {
        const response = await fetch(`${API_BASE_URL}/Vehicle/${id}`);
        if (!response.ok) {
            throw new Error("Cannot find car with id ${id}");
        }
        const dto = response.json();
        return dto.data;
    },
}