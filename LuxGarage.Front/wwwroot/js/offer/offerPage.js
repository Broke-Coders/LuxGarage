import { OfferService } from "../offerService.js";
import { CarService }   from "../carService.js";
import { RentalService } from "../rentalService.js";

const BASE_URL = "http://localhost:5054";

export let vehicleId   = 1;
export let pricePerDay = 0;
export let bookedDates = [];

export async function initOfferPage(offerId, onReady) {
   try {
      const offer = await OfferService.getOfferById(offerId);
      if (!offer) throw new Error("Offer not found");

      vehicleId   = offer.vehicleId;
      pricePerDay = offer.price;

      document.title = `${offer.brand} ${offer.model} | LuxGarage Offer`;
      document.querySelector(".offer-main-title").textContent = offer.title;
      document.querySelector(".section-content p").textContent = offer.description;

      // Vehicle Info Table
      const infoSections = document.querySelectorAll(".offer-section");
      infoSections[2].querySelector(".info-table").innerHTML = `
         <div class="info-row"><span>Class/category</span><span>${offer.bodyType}</span></div>
         <div class="info-row"><span>Make</span><span>${offer.brand}</span></div>
         <div class="info-row"><span>Model</span><span>${offer.model}</span></div>
         <div class="info-row"><span>Color</span><span>${offer.color}</span></div>
         <div class="info-row"><span>Availability</span><span>${offer.status}</span></div>
      `;

      // Performance Table
      infoSections[3].querySelector(".info-table").innerHTML = `
         <div class="info-row"><span>Engine</span><span>${offer.engine}</span></div>
         <div class="info-row"><span>Power</span><span>${offer.horsepower} HP</span></div>
         <div class="info-row"><span>0-100 km/h</span><span>${offer.zeroToHundred}</span></div>
         <div class="info-row"><span>Year</span><span>${offer.year}</span></div>
      `;

      // Modal Summary
      document.getElementById("sum-brand").textContent  = offer.brand;
      document.getElementById("sum-model").textContent  = offer.model;
      document.getElementById("sum-engine").textContent = offer.engine;
      document.getElementById("sum-seats").textContent  =
         ["SUV", "Sedan", "Wagon"].includes(offer.bodyType) ? "5" : "2";
      document.getElementById("sum-perf").textContent   = offer.zeroToHundred;

      await _initGallery(vehicleId);
      await _loadBookedDates(vehicleId);

      if (onReady) onReady();
   } catch (error) {
      console.error("Error loading offer data:", error);
      alert("Could not load offer data.");
   }
}

async function _initGallery(vehicleId) {
   const images      = await CarService.getImagesByVehicleId(vehicleId);
   const mainImg     = document.querySelector(".main-img");
   const thumbGallery = document.querySelector(".thumbnail-gallery");

   if (!images || images.length === 0) return;

   let currentImgIdx = 0;
   mainImg.src        = `${BASE_URL}${images[0].url}`;
   thumbGallery.innerHTML = "";

   const updateGallery = (index) => {
      currentImgIdx = index;
      mainImg.src   = `${BASE_URL}${images[currentImgIdx].url}`;
      document.querySelectorAll(".thumb").forEach((t, i) => {
         t.classList.toggle("active", i === currentImgIdx);
         if (i === currentImgIdx) {
            t.scrollIntoView({ behavior: "smooth", block: "nearest", inline: "center" });
         }
      });
   };

   images.forEach((img, idx) => {
      const thumb = document.createElement("img");
      thumb.src   = `${BASE_URL}${img.url}`;
      thumb.alt   = `Thumb ${idx + 1}`;
      thumb.classList.add("thumb");
      if (idx === 0) thumb.classList.add("active");
      thumb.onclick = () => updateGallery(idx);
      thumbGallery.appendChild(thumb);
   });

   const prevBtn = document.querySelector(".nav-btn.prev");
   const nextBtn = document.querySelector(".nav-btn.next");

   if (prevBtn) {
      prevBtn.onclick = () => {
         let idx = currentImgIdx - 1;
         if (idx < 0) idx = images.length - 1;
         updateGallery(idx);
      };
   }
   if (nextBtn) {
      nextBtn.onclick = () => {
         let idx = currentImgIdx + 1;
         if (idx >= images.length) idx = 0;
         updateGallery(idx);
      };
   }
}

async function _loadBookedDates(vehicleId) {
   try {
      const datesResponse = await RentalService.getUnavailableDates(vehicleId);

      let dates = [];
      if (Array.isArray(datesResponse)) {
         dates = datesResponse;
      } else if (datesResponse?.data && Array.isArray(datesResponse.data)) {
         dates = datesResponse.data;
      } else if (datesResponse?.value && Array.isArray(datesResponse.value)) {
         dates = datesResponse.value;
      } else if (datesResponse?.data?.value && Array.isArray(datesResponse.data.value)) {
         dates = datesResponse.data.value;
      }

      bookedDates.length = 0;
      dates.forEach((d) => {
         const start = new Date(d.startDate);
         const end   = new Date(d.endDate);
         start.setHours(0, 0, 0, 0);
         end.setHours(0, 0, 0, 0);
         bookedDates.push({ start, end });
      });
   } catch (e) {
      console.error("Could not fetch unavailable dates", e);
   }
}