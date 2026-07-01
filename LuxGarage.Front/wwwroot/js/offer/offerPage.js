import { OfferService } from "../offerService.js";
import { CarService }   from "../carService.js";
import { RentalService } from "../rentalService.js";
import { InsuranceService } from "../insuranceService.js";

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
      await _loadReadonlyInsurances();

      if (onReady) onReady();
   } catch (error) {
      console.error("Error loading offer data:", error);
      alert("Could not load offer data.");
   }
}

const INSURANCE_METADATA = {
  "Basic Insurance": {
    icon: "shield-outline",
    desc: "Standard coverage for minor scratches."
  },
  "Premium Shield": {
    icon: "shield-checkmark-outline",
    desc: "Full protection. Zero deductible."
  },
  "Additional Driver": {
    icon: "person-add-outline",
    desc: "Share the thrill with a friend."
  },
  "Pro Cleaning": {
    icon: "sparkles-outline",
    desc: "Return dirty, we'll handle the rest."
  }
};

async function _loadReadonlyInsurances() {
   const container = document.getElementById("extras-readonly-container");
   if (!container) return;

   try {
      const insurances = await InsuranceService.getAll();
      const activeInsurances = insurances.filter(i => i.isActive);

      if (activeInsurances.length === 0) {
         container.innerHTML = `<p style="grid-column: 1 / -1; text-align: center; color: #8d8a7c; font-size: 1.5rem;">No extra additions available.</p>`;
         return;
      }

      container.innerHTML = activeInsurances.map((ins) => {
         const metadata = INSURANCE_METADATA[ins.name] || {
            icon: "shield-outline",
            desc: "Optional addition for your rental."
         };

         const priceLabel = ins.pricePerDay === 0 ? "Included" : `+${ins.pricePerDay} PLN / day`;

         return `
            <div class="extra-card-readonly">
               <div class="extra-icon">
                  <ion-icon name="${metadata.icon}"></ion-icon>
               </div>
               <h4>${ins.name}</h4>
               <p>${metadata.desc}</p>
               <span class="extra-price">${priceLabel}</span>
            </div>
         `;
      }).join("");
   } catch (error) {
      console.error("Error loading readonly insurances:", error);
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