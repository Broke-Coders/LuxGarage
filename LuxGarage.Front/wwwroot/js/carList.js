import { CarService } from "./carService.js";
import { OfferService } from "./offerService.js";

/**
 * @file carList.js
 * @module CarList
 * @description This script is responsible for fetching the list of cars from the API and displaying them on the page. 
 * It listens for the DOMContentLoaded event and button click events, then calls the CarService to 
 * get all cars, and dynamically creates and updates container's innerHTML to show each car's details. 
 * If there are no cars or if an error occurs, it displays an appropriate message.
 */
const container = document.getElementById("cars-container");
const searchButtons = document.querySelectorAll(".search-button");

/** 
 * Hardcoded fleet of 12 premium cars for demonstration purposes.
 * These follow the structure of VehicleListItemResponse but include realistic data.
 */
const hardcodedCars = [
    {
        id: 1,
        brandName: "Porsche",
        modelName: "911 GT3 RS",
        bodyName: "Coupe",
        engine: "4.0L Flat-6",
        horsepower: 525,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "3.2s",
        mileage: 1200,
        licensePlate: "S GT 911",
        pricePerDay: 2500,
        image: "./images/cars/niklas-bischop-KZayf7xRScI-unsplash.jpg"
    },
    {
        id: 2,
        brandName: "Mercedes-AMG",
        modelName: "GT Black Series",
        bodyName: "Coupe",
        engine: "4.0L V8 Biturbo",
        horsepower: 730,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "3.2s",
        mileage: 850,
        licensePlate: "MA GT 730",
        pricePerDay: 3000,
        image: "./images/cars/flavien-s_E1TRPiId0-unsplash.jpg"
    },
    {
        id: 3,
        brandName: "BMW",
        modelName: "M3 Competition",
        bodyName: "Sedan",
        engine: "3.0L Straight-6",
        horsepower: 510,
        seats: 5,
        driveType: "xDrive (AWD)",
        zeroToHundred: "3.5s",
        mileage: 4500,
        licensePlate: "M WM 3000",
        pricePerDay: 1200,
        image: "./images/cars/pexels-habib-hosseini-2613461.jpg"
    },
    {
        id: 4,
        brandName: "BMW",
        modelName: "M4 CSL",
        bodyName: "Coupe",
        engine: "3.0L Straight-6",
        horsepower: 550,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "3.7s",
        mileage: 320,
        licensePlate: "M CS 444",
        pricePerDay: 1400,
        image: "./images/cars/pexels-mohit-hambiria-92377455-36407338.jpg"
    },
    {
        id: 5,
        brandName: "Brabus",
        modelName: "G900 Rocket Edition",
        bodyName: "SUV",
        engine: "4.5L V8 Biturbo",
        horsepower: 900,
        seats: 5,
        driveType: "AWD",
        zeroToHundred: "3.7s",
        mileage: 150,
        licensePlate: "B RS 900",
        pricePerDay: 4000,
        image: "./images/cars/dextar-vision-YYXRSgxFAxA-unsplash.jpg"
    },
    {
        id: 6,
        brandName: "Koenigsegg",
        modelName: "Jesko Absolut",
        bodyName: "Hypercar",
        engine: "5.0L V8 Biturbo",
        horsepower: 1600,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "2.5s",
        mileage: 50,
        licensePlate: "FAST 1",
        pricePerDay: 15000,
        image: "./images/cars/mclaren.jpg" 
    },
    {
        id: 7,
        brandName: "Ferrari",
        modelName: "296 GTB",
        bodyName: "Coupe",
        engine: "3.0L V6 Hybrid",
        horsepower: 830,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "2.9s",
        mileage: 1100,
        licensePlate: "F 296 IT",
        pricePerDay: 3500,
        image: "./images/cars/488gtb.jpg"
    },
    {
        id: 8,
        brandName: "Lamborghini",
        modelName: "Revuelto",
        bodyName: "Coupe",
        engine: "6.5L V12 Hybrid",
        horsepower: 1015,
        seats: 2,
        driveType: "AWD",
        zeroToHundred: "2.5s",
        mileage: 210,
        licensePlate: "L RB 1015",
        pricePerDay: 4500,
        image: "./images/cars/pexels-introspectivedsgn-4077271.jpg"
    },
    {
        id: 9,
        brandName: "Audi",
        modelName: "RS6 Avant",
        bodyName: "Wagon",
        engine: "4.0L V8 Biturbo",
        horsepower: 630,
        seats: 5,
        driveType: "Quattro (AWD)",
        zeroToHundred: "3.4s",
        mileage: 8200,
        licensePlate: "IN RS 660",
        pricePerDay: 1100,
        image: "./images/cars/nsx.jpg" 
    },
    {
        id: 10,
        brandName: "McLaren",
        modelName: "Artura",
        bodyName: "Coupe",
        engine: "3.0L V6 Hybrid",
        horsepower: 680,
        seats: 2,
        driveType: "RWD",
        zeroToHundred: "3.0s",
        mileage: 1500,
        licensePlate: "MC ART 1",
        pricePerDay: 2800,
        image: "./images/cars/mclaren.jpg"
    },
    {
        id: 11,
        brandName: "Aston Martin",
        modelName: "DBS Volante",
        bodyName: "Cabriolet",
        engine: "5.2L V12 Biturbo",
        horsepower: 715,
        seats: 4,
        driveType: "RWD",
        zeroToHundred: "3.6s",
        mileage: 3400,
        licensePlate: "AM DBS 07",
        pricePerDay: 2200,
        image: "./images/cars/flavien-s_E1TRPiId0-unsplash.jpg"
    },
    {
        id: 12,
        brandName: "Rolls-Royce",
        modelName: "Cullinan",
        bodyName: "SUV",
        engine: "6.75L V12 Biturbo",
        horsepower: 600,
        seats: 5,
        driveType: "AWD",
        zeroToHundred: "4.8s",
        mileage: 12000,
        licensePlate: "RR LUX 1",
        pricePerDay: 3800,
        image: "./images/cars/dextar-vision-YYXRSgxFAxA-unsplash.jpg"
    }
];

/** @type {Set<string>} Active body type filters */
const activeFilters = new Set();
let allCars = [];

/**
 * Displays a list of cars in the container element.
 * @param {Array} cars - Array of car objects to display
 */
function displayCars(cars) {
    container.innerHTML = "";

    if (!cars || cars.length === 0) {
        container.innerHTML = "<div id='loading-message'>No cars found matching your criteria</div>";
        return;
    }

    cars.forEach(car => {
        const carCard = `
               <div class="car">
                  <img
                     src="${car.image}" 
                     class="car-img"
                     alt="${car.brandName} ${car.modelName}"
                  />
                  <div class="car-content">
                     <div class="car-tags">
                        <span class="tag tag--origin">${car.bodyName}</span>
                        <span class="tag tag--category">${car.horsepower} HP</span>
                        <span class="tag tag--price">${car.pricePerDay.toLocaleString()} PLN / day</span>
                     </div>
                     <p class="car-title">${car.brandName} ${car.modelName}</p>
                     <ul class="car-attributes">
                        <li class="car-attribute">
                           <ion-icon class="car-icon" name="speedometer-outline"></ion-icon>
                           <span>Power: <strong>${car.horsepower}</strong> hp / <strong>${car.engine}</strong></span>
                        </li>
                        <li class="car-attribute">
                           <ion-icon class="car-icon" name="people-outline"></ion-icon>
                           <span>Seats: <strong>${car.seats}</strong> / Drive: <strong>${car.driveType}</strong></span>
                        </li>
                         <li class="car-attribute">
                           <ion-icon class="car-icon" name="flash-outline"></ion-icon>
                           <span>0-100: <strong>${car.zeroToHundred}</strong> / Body: <strong>${car.bodyName}</strong></span>
                        </li>
                     </ul>
                     <a href="offer.html?id=${car.id}" class="btn-car">Rent Now</a>
                  </div>
               </div>`
        container.innerHTML += carCard;
    });
}

/**
 * Filters the combined car list based on active filters.
 */
function filterCars() {
    if (activeFilters.size === 0) {
        displayCars(allCars);
        return;
    }

    const filtered = allCars.filter(car => activeFilters.has(car.bodyName));
    displayCars(filtered);
}

async function loadCars() {
    try {
        const apiOffers = await OfferService.getAllOffers();
        const dynamicCars = apiOffers.map(offer => ({
            id: offer.id,
            brandName: offer.brand,
            modelName: offer.model,
            bodyName: offer.bodyName,
            engine: offer.engine,
            horsepower: offer.horsepower,
            seats: offer.model.includes("Urus") ? 5 : 2,
            driveType: offer.brand === "Lamborghini" ? "AWD" : "RWD",
            zeroToHundred: offer.zeroToHundred,
            mileage: parseInt(offer.mileage),
            licensePlate: "",
            pricePerDay: offer.price,
            image: `http://localhost:5054${offer.primaryImageUrl}`
        }));

        allCars = [...dynamicCars, ...hardcodedCars];
        displayCars(allCars);
    } catch (error) {
        console.error("Error loading cars from API:", error);
        allCars = [...hardcodedCars];
        displayCars(allCars);
    }
}

document.addEventListener("DOMContentLoaded", () => {
    loadCars();
});

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
        
        filterCars();
    });
});