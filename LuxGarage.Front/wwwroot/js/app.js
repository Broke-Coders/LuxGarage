const API_BASE_URL = 'http://localhost:5054/api';

async function fetchVehicles() {
    try {
        const response = await fetch(`${API_BASE_URL}/vehicles`);
        if (!response.ok) {
            throw new Error(`API Error: ${response.status} ${response.statusText}`);
        }

        const vehicles = await response.json();
        return vehicles;
    } catch (error) {
        console.error('Error fetching vehicles:', error);
        return [];
    }
}

function createVehicleCard(vehicle) {
    return `
        <div class="car-card">
            <img src="./images/car-placeholder.png" alt="${vehicle.brandName} ${vehicle.modelName}" class="car-img" />
            <h3 class="car-title">${vehicle.brandName} ${vehicle.modelName}</h3>
            <p class="car-description">
                <strong>Body:</strong> ${vehicle.bodyName}<br>
                <strong>Color:</strong> ${vehicle.colorName}<br>
                <strong>Power:</strong> ${vehicle.horsepower} HP<br>
                <strong>Mileage:</strong> ${vehicle.mileage} km<br>
                <strong>Plate:</strong> ${vehicle.licensePlate}
            </p>
        </div>
    `;
}

async function loadVehicles() {
    const carsContainer = document.querySelector('.cars-container');

    if (!carsContainer) {
        console.error('Cars container element not found');
        return;
    }

    carsContainer.innerHTML = '<p>Imagine tyle czekac na odpowiedz...</p>';

    const vehicles = await fetchVehicles();

    if (vehicles.length === 0) {
        carsContainer.innerHTML = '<p>THERE ARE NO CARS, literally</p>';
        return;
    }

    let vehiclesHTML = '';
    for (let vehicle of vehicles) {
        vehiclesHTML += createVehicleCard(vehicle);
    }

    carsContainer.innerHTML = vehiclesHTML;
}

document.addEventListener('DOMContentLoaded', () => {
    loadVehicles();
});
