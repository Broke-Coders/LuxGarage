import { CarService } from "./carService.js";


/**
 * This script is responsible for fetching the list of cars from the API and displaying them on the page. 
 * It listens for the DOMContentLoaded event, then calls the CarService to 
 * get all cars, and dynamically creates HTML elements to show each car's details. 
 * If there are no cars or if an error occurs, it displays an appropriate message.
 */
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
                <p>Year: ${car.year}</p>
            `;
        container.appendChild(carCard);
        });
    } catch (e) {
        console.error("Error:", e);
        container.innerHTML = "<p>Error occured while loading vehicle</p>";
    }
}
);