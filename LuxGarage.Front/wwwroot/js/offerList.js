import { OfferService } from "./offerService.js";

const container = document.getElementById("offers-container");

function displayOffers(offers) {
    container.innerHTML = "";

    if (!offers || offers.length === 0) {
        container.innerHTML = "<p>No offers in database</p>";
        return;
    }

    offers.forEach(offer => {
        const card = document.createElement("div");
        card.className = "offer-card";
        card.innerHTML = `
            <h2>${offer.name ?? "Unnamed offer"}</h2>
            <p>${offer.description ?? ""}</p>
        `;
        container.appendChild(card);
    });
}

document.addEventListener("DOMContentLoaded", async () => {
    try {
        const offers = await OfferService.getAllOffers();
        displayOffers(offers);
    } catch (e) {
        console.error(e);
        container.innerHTML = "<p>Error occurred while loading offers</p>";
    }
});