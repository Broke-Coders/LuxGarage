import { CarService } from "./carService.js";

document.addEventListener("DOMContentLoaded", async () => {

    const container = document.getElementById("cars-container");

    try {
        const carsArray = await CarService.getAllCars();

        container.innerHTML = "";

        if (!carsArray || carsArray.length === 0) {
            container.innerHTML = "<p>No cars in database</p>";
            return;
        }
        
        carsArray.forEach(car => {
            const carCard = document.createElement("div");
            carCard.className = "car-card";
        carCard.innerHTML = `
                <h2>${car.brandName} ${car.modelName} ${car.horsepower}</h2>
                <p>Rok: ${car.year}</p>
            `;
        container.appendChild(carCard);
        });
    } catch (e) {
        console.error("Error:", e);
        container.innerHTML = "<p>Error occured while loading vehicle</p>";
    }
}
);