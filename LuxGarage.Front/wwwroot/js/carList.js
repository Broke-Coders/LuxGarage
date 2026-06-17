import { CarService } from "./carService.js";
import { OfferService } from "./offerService.js";
import { RentalService } from "./rentalService.js";

/**
 * @file carList.js
 * @module CarList
 */
const container = document.getElementById("cars-container");
const searchButtons = document.querySelectorAll(".search-button");
const searchInput = document.getElementById("car-search-input");

const brandDropdown = document.getElementById("brand-dropdown");
const brandSelectBox = document.querySelector(".select-box");
const brandSelect = document.getElementById("brand-select");
const minPriceInput = document.getElementById("min-price");
const maxPriceInput = document.getElementById("max-price");
const startDateInput = document.getElementById("start-date");
const endDateInput = document.getElementById("end-date");
const clearFiltersBtn = document.getElementById("clear-filters");

let allCars = [];
let allRentals = []; // Used for availability check
const activeBodyFilters = new Set();
const activeBrandFilters = new Set();
let currentSearchTerm = "";

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
                        <span class="tag tag--price">${car.pricePerDay.toLocaleString()} PLN / day</span>
                     </div>
                     <p class="car-title">${car.brandName} ${car.modelName}</p>
                     <ul class="car-attributes">
                        <li class="car-attribute">
                           <ion-icon class="car-icon" name="speedometer-outline"></ion-icon>
                           <span>Power: <strong>${car.horsepower} hp</strong> / <strong>${car.engine}</strong></span>
                        </li>
                        <li class="car-attribute">
                           <ion-icon class="car-icon" name="people-outline"></ion-icon>
                           <span>Seats: <strong>${car.seats}</strong></span>
                        </li>
                        <li class="car-attribute">
                           <ion-icon class="car-icon" name="car-sport-outline"></ion-icon>
                           <span>Drive: <strong>${car.driveType}</strong></span>
                        </li>
                         <li class="car-attribute">
                           <ion-icon class="car-icon" name="timer-outline"></ion-icon>
                           <span>0-100 km/h: <strong>${car.zeroToHundred}</strong></span>
                        </li>
                     </ul>
                     <a href="offer.html?id=${car.id}" class="btn-car">Rent Now</a>
                  </div>
               </div>`
        container.innerHTML += carCard;
    });
}

function isCarAvailable(carId, startDateStr, endDateStr) {
    if (!startDateStr || !endDateStr) return true;
    
    const checkStart = new Date(startDateStr);
    const checkEnd = new Date(endDateStr);
    
    // Safety check: end date must be after start date
    if (checkStart > checkEnd) return false;

    // Filter rentals for this car
    const carRentals = allRentals.filter(r => r.vehicleId === carId && (r.status === 'Active' || r.status === 'Pending'));
    
    for (let rental of carRentals) {
        const rStart = new Date(rental.startingTime);
        const rEnd = new Date(rental.appointedReturnTime);
        
        // Overlap logic: (StartA <= EndB) and (EndA >= StartB)
        if (checkStart <= rEnd && checkEnd >= rStart) {
            return false; // Found an overlap, not available
        }
    }
    return true; // No overlaps found
}

function filterCars() {
    let filtered = allCars;

    // 1. Text Search Filter
    if (currentSearchTerm) {
        const search = currentSearchTerm.toLowerCase();
        filtered = filtered.filter(car => 
            car.brandName.toLowerCase().includes(search) || 
            car.modelName.toLowerCase().includes(search)
        );
    }

    // 2. Body Type Filter
    if (activeBodyFilters.size > 0) {
        filtered = filtered.filter(car => activeBodyFilters.has(car.bodyName));
    }

    // 3. Brand Filter (Multi-select)
    if (activeBrandFilters.size > 0) {
        filtered = filtered.filter(car => activeBrandFilters.has(car.brandName));
    }

    // 4. Price Filter
    const minPrice = parseFloat(minPriceInput.value);
    const maxPrice = parseFloat(maxPriceInput.value);
    
    if (!isNaN(minPrice)) {
        filtered = filtered.filter(car => car.pricePerDay >= minPrice);
    }
    if (!isNaN(maxPrice)) {
        filtered = filtered.filter(car => car.pricePerDay <= maxPrice);
    }

    // 5. Availability Filter
    const startD = startDateInput.value;
    const endD = endDateInput.value;
    if (startD && endD) {
        filtered = filtered.filter(car => isCarAvailable(car.vehicleId, startD, endD));
    }

    displayCars(filtered);
}

function renderBrandDropdown(brands) {
    if (!brandDropdown) return;
    brandDropdown.innerHTML = "";
    
    brands.forEach(brand => {
        const label = document.createElement("label");
        label.className = "checkbox-label";
        
        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.value = brand;
        checkbox.checked = activeBrandFilters.has(brand);
        
        checkbox.addEventListener("change", (e) => {
            if (e.target.checked) {
                activeBrandFilters.add(brand);
            } else {
                activeBrandFilters.delete(brand);
            }
            updateBrandSelectBoxText();
            filterCars();
        });
        
        const text = document.createTextNode(` ${brand}`);
        label.appendChild(checkbox);
        label.appendChild(text);
        
        brandDropdown.appendChild(label);
    });
}

function updateBrandSelectBoxText() {
    if (!brandSelectBox) return;
    if (activeBrandFilters.size === 0) {
        brandSelectBox.textContent = "Select brands...";
    } else if (activeBrandFilters.size === 1) {
        brandSelectBox.textContent = [...activeBrandFilters][0];
    } else {
        brandSelectBox.textContent = `${activeBrandFilters.size} brands selected`;
    }
}

async function loadData(searchTerm = "") {
    try {
        currentSearchTerm = searchTerm;
        
        // Fetch offers and rentals concurrently
        const [apiOffers, apiRentals] = await Promise.all([
            OfferService.getAllOffers({ searchTerm }),
            // Fetch rentals for availability filter - using open GET if available, otherwise just gracefully fallback to empty array
            fetch("http://localhost:5054/api/rental").then(res => res.ok ? res.json() : []).catch(() => [])
        ]);

        allRentals = apiRentals;
        
        allCars = apiOffers.map(offer => {
            const isSedanOrSUV = offer.bodyName === "Sedan" || offer.bodyName === "SUV" || offer.bodyName === "Wagon";
            const isAWD = offer.brand === "Lamborghini" || offer.brand === "Bentley" || offer.brand === "Audi" || (offer.brand === "Mercedes-Benz" && offer.model === "S580") || (offer.brand === "Mercedes-AMG" && offer.model === "GT Black Series");
            
            return {
                id: offer.id,
                vehicleId: offer.vehicleId, // needed for availability check
                brandName: offer.brand,
                modelName: offer.model,
                bodyName: offer.bodyName,
                engine: offer.engine,
                horsepower: offer.horsepower,
                seats: isSedanOrSUV ? 5 : 2,
                driveType: isAWD ? "AWD" : "RWD",
                zeroToHundred: offer.zeroToHundred,
                mileage: parseInt(offer.mileage),
                licensePlate: "",
                pricePerDay: offer.price,
                image: `http://localhost:5054${offer.primaryImageUrl}`
            };
        });

        // Extract unique brands for filtering
        const uniqueBrands = [...new Set(allCars.map(car => car.brandName))].sort();
        renderBrandDropdown(uniqueBrands);

        filterCars();
    } catch (error) {
        console.error("Error loading data from API:", error);
        allCars = [];
        filterCars();
    }
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const handleSearch = debounce((e) => {
    loadData(e.target.value);
});

// Event Listeners Initialization
document.addEventListener("DOMContentLoaded", () => {
    loadData();
    
    if (searchInput) {
        searchInput.addEventListener("input", handleSearch);
    }
    
    // Body type buttons
    searchButtons.forEach(searchButton => {
        searchButton.addEventListener("click", () => {
            const filterType = searchButton.id;
            if (activeBodyFilters.has(filterType)) {
                activeBodyFilters.delete(filterType);
                searchButton.classList.remove("active");
            } else {
                activeBodyFilters.add(filterType);
                searchButton.classList.add("active");
            }
            filterCars();
        });
    });

    // Custom Select Dropdown Toggle
    if (brandSelectBox) {
        brandSelectBox.addEventListener("click", (e) => {
            brandSelect.classList.toggle("active");
            e.stopPropagation(); // prevent document click from closing immediately
        });
    }

    // Close dropdown when clicking outside
    document.addEventListener("click", (e) => {
        if (brandSelect && !brandSelect.contains(e.target)) {
            brandSelect.classList.remove("active");
        }
    });
    
    // Stop propagation on dropdown content so clicking checkboxes doesn't close it
    if(brandDropdown) {
        brandDropdown.addEventListener("click", (e) => {
            e.stopPropagation();
        });
    }

    // Advanced Inputs
    [minPriceInput, maxPriceInput, startDateInput, endDateInput].forEach(input => {
        if (input) {
            input.addEventListener("input", filterCars);
        }
    });

    // Clear Filters Button
    if (clearFiltersBtn) {
        clearFiltersBtn.addEventListener("click", () => {
            // Reset active sets
            activeBodyFilters.clear();
            activeBrandFilters.clear();
            currentSearchTerm = "";
            
            // Reset inputs
            if (searchInput) searchInput.value = "";
            if (minPriceInput) minPriceInput.value = "";
            if (maxPriceInput) maxPriceInput.value = "";
            if (startDateInput) startDateInput.value = "";
            if (endDateInput) endDateInput.value = "";
            
            // Reset UI elements
            searchButtons.forEach(btn => btn.classList.remove("active"));
            updateBrandSelectBoxText();
            const checkboxes = brandDropdown.querySelectorAll("input[type='checkbox']");
            checkboxes.forEach(cb => cb.checked = false);
            
            filterCars();
        });
    }
});