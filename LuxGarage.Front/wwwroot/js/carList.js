import { CarService } from "./carService.js";

/**
 * @file carList.js
 * @module CarList
 * @description This script is responsible for fetching the list of cars from the API and displaying them on the page. 
 * It listens for the DOMContentLoaded event and button click events, then calls the CarService to 
 * get all cars, and dynamically creates and updates container's innerHTML to show each car's details. 
 * If there are no cars or if an error occurs, it displays an appropriate message.
 */
const searchButtons = document.querySelectorAll(".search-button");
const container = document.getElementById("cars-container");
/** @type {Array} Array storing all available cars fetched from the API */
let carsArray = [];

/**
 * Maps vehicle body type names to their corresponding database IDs
 * @type {Object.<string, number>}
 * @example
 * bodyTypeMap["Sedan"] => 1
 * bodyTypeMap["Hatchback"] => 2
 */
const bodyTypeMap = {
    "Sedan": 1,
    "Hatchback": 2,
    "Coupe": 3,
    "Wagon": 4,
    "Cabriolet": 5,
    "Roadster": 6,
    "Pickup": 7,
    "Van": 8
};

/**
 * Displays a list of cars in the container element.
 * Clears previous content and renders car cards for each vehicle in the provided array.
 * If no cars are provided or the array is empty, displays a "No cars in database" message.
 * @param {Array} cars - Array of car objects to display
 * @param {string} cars[].brandName - The brand/manufacturer of the car
 * @param {string} cars[].modelName - The model name of the car
 * @param {number} cars[].horsepower - The horsepower of the car
 * @param {number} cars[].year - The manufacturing year of the car
 * @returns {void}
 */
function displayCars(cars) {
    container.innerHTML = "";

    if (!cars || cars.length === 0) {
        container.innerHTML = "<p>No cars in database</p>";
        return;
    }

    cars.forEach(car => {
        const carCard = `<div class="car-card">
                            <h2>${car.brandName} ${car.modelName} ${car.horsepower}</h2>
                            <p>Year: ${car.year}</p>
                            <button class="car-btn" id="${car.modelName}-btn">Rent Now</button>
                        </div>`
        container.innerHTML += carCard;
    });
}

document.addEventListener("DOMContentLoaded", async () => {
    try {
        carsArray = await CarService.getAllCars();
        displayCars(carsArray);
    } catch (e) {
        console.error("Error:", e);
        container.innerHTML = "<p>Error occured while loading vehicle</p>";
    }
}
);


/** @type {Set<string>} Set storing currently active filter names (body types selected by user) */
const activeFilters = new Set();

/**
 * Filters cars based on currently active body type filters.
 * If no filters are active, displays all cars.
 * If filters are active, fetches filtered cars from the API and displays them.
 * @async
 * @function filterCars
 * @returns {Promise<void>} - Does not return a value, updates the DOM with filtered results
 * @throws {Error} - Logs error if filtering request fails
 */
async function filterCars() {
    try {
        if (activeFilters.size === 0) {
            displayCars(carsArray);
            return;
        }

        const bodyTypeIds = Array.from(activeFilters).map(filter => bodyTypeMap[filter]);
        
        const filteredCars = await CarService.getCarsByBodyTypes(bodyTypeIds);
        displayCars(filteredCars);
    } catch (e) {
        console.error("Error filtering cars:", e);
        container.innerHTML = "<p>Error occurred while filtering vehicles</p>";
    }
}

searchButtons.forEach(searchButton => {
    searchButton.addEventListener("click", () => {
        const filterType = searchButton.id;
        
        if (activeFilters.has(filterType)) {
            activeFilters.delete(filterType);
            searchButton.classList.remove("active");
        } else {
            activeFilters.add(filterType);
            searchButton.classList.add("active");
        }
        
        console.log("Active filters:", Array.from(activeFilters));
        filterCars();
    });
})