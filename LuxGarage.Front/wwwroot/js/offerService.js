const API_BASE_URL = 'http://localhost:5054/api';

export const OfferService = {
    async getAllOffers(request = {}) {
        const params = new URLSearchParams();

        if (request.sortBy) params.append("SortBy", request.sortBy);
        if (request.descending !== undefined) params.append("Descending", request.descending);

        const query = params.toString() ? `?${params.toString()}` : "";
        const response = await fetch(`${API_BASE_URL}/offers${query}`);

        if (!response.ok) {
            throw new Error("Failed getting list of offers");
        }

        return await response.json();
    },

    async getOfferById(id) {
        const response = await fetch(`${API_BASE_URL}/offers/${id}`);

        if (!response.ok) {
            throw new Error(`Cannot find offer with id ${id}`);
        }

        return await response.json();
    }
};