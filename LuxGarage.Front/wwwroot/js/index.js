import { OfferService } from "./offerService.js";

async function initMainPage() {
    const slidersContainer = document.getElementById("sliders-container");
    if (!slidersContainer) return;

    try {
        const offers = await OfferService.getAllOffers();
        
        // Categories we want to display as separate rows
        const categoriesToShow = [
            { id: "Coupe", label: "Performance & Sports" },
            { id: "SUV", label: "Luxury SUVs" },
            { id: "Sedan", label: "Premium Sedans" },
            { id: "Cabriolet", label: "Open-Top Thrills" }
        ];

        // Clear loading or static content
        slidersContainer.innerHTML = "";

        categoriesToShow.forEach(cat => {
            const catOffers = offers.filter(o => o.bodyName === cat.id);
            if (catOffers.length > 0) {
                renderCategorySlider(cat, catOffers, slidersContainer);
            }
        });

        // Initialize all swipers
        initializeSwipers();

    } catch (error) {
        console.error("Error loading main page offers:", error);
    }
}

function renderCategorySlider(category, offers, container) {
    const section = document.createElement("div");
    section.className = "category-row";
    
    const id = `swiper-${category.id.toLowerCase()}`;
    
    section.innerHTML = `
        <div class="container">
            <h3 class="category-title">${category.label}</h3>
        </div>
        <div class="container swiper-container-wrapper">
            <div class="swiper car-swiper" id="${id}">
                <div class="swiper-wrapper">
                    ${offers.map(offer => renderOfferSlide(offer)).join('')}
                </div>
                <div class="swiper-button-prev"></div>
                <div class="swiper-button-next"></div>
            </div>
        </div>
    `;
    
    container.appendChild(section);
}

function renderOfferSlide(offer) {
    return `
        <div class="swiper-slide">
            <div class="car">
                <img
                    src="http://localhost:5054${offer.primaryImageUrl}"
                    class="car-img"
                    alt="${offer.brand} ${offer.model}"
                    onerror="this.src='./images/cars/mclaren.jpg'"
                />
                <div class="car-content">
                    <div class="car-tags">
                        <span class="tag tag--origin">${offer.brand}</span>
                        <span class="tag tag--category">${offer.bodyName}</span>
                    </div>
                    <p class="car-title">${offer.brand} ${offer.model}</p>
                    <ul class="car-attributes">
                        <li class="car-attribute">
                            <ion-icon class="car-icon" name="speedometer-outline"></ion-icon>
                            <span>Power: <strong>${offer.horsepower}</strong> hp</span>
                        </li>
                        <li class="car-attribute">
                            <ion-icon class="car-icon" name="timer-outline"></ion-icon>
                            <span>0-100: <strong>${offer.zeroToHundred}</strong></span>
                        </li>
                        <li class="car-attribute">
                            <ion-icon class="car-icon" name="flame-outline"></ion-icon>
                            <span>Engine: <strong>${offer.engine}</strong></span>
                        </li>
                    </ul>
                    <div class="car-footer">
                        <span class="car-price"><strong>${offer.price.toLocaleString()}</strong> PLN / day</span>
                        <a href="offer.html?id=${offer.id}" class="btn-car-small">Rent Now</a>
                    </div>
                </div>
            </div>
        </div>
    `;
}

function initializeSwipers() {
    document.querySelectorAll(".car-swiper").forEach(el => {
        new Swiper(`#${el.id}`, {
            direction: "horizontal",
            loop: false, // Loop false is better for small data sets
            slidesPerView: 1,
            spaceBetween: 20,
            navigation: {
                nextEl: `#${el.id} ~ .swiper-button-next`,
                prevEl: `#${el.id} ~ .swiper-button-prev`,
            },
            breakpoints: {
                640: { slidesPerView: 2, spaceBetween: 20 },
                1024: { slidesPerView: 3, spaceBetween: 30 },
            },
            grabCursor: true,
        });
    });
}

document.addEventListener("DOMContentLoaded", initMainPage);
