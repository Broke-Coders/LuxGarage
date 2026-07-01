import { RentalService }  from "../rentalService.js";
import { UserService }    from "../userService.js";
import { PaymentService } from "../paymentService.js";
import { InsuranceService } from "../insuranceService.js";
import { startDate, endDate, pricePerDayRef } from "./offerCalendar.js";
import { vehicleId } from "./offerPage.js";

// DOM refs
const modal               = document.getElementById("booking-modal");
const closeBtn            = document.querySelector(".close-modal");
const steps               = document.querySelectorAll(".form-step");
const stepperItems        = document.querySelectorAll(".step");
const nextBtns            = document.querySelectorAll(".next-step");
const prevBtns            = document.querySelectorAll(".prev-step");
const finalPriceDisplay   = document.getElementById("final-total-price");
const displayDuration     = document.getElementById("display-duration");
const displayBasePrice    = document.getElementById("display-base-price");
const confirmBookingFinal = document.getElementById("confirm-booking-final");
const bookBtnSidebar      = document.querySelector(".btn-book-sidebar");

// Customer fields
const custFirstName = document.getElementById("cust-firstname");
const custLastName  = document.getElementById("cust-lastname");
const custEmail     = document.getElementById("cust-email");
const custPhone     = document.getElementById("cust-phone");
const custLicense   = document.getElementById("cust-license");
const phoneError    = document.getElementById("phone-error");

let currentStep = 1;

// ── Public init ───────────────────────────────────────────────────────────────
export function initBooking() {
   _bindOpenModal();
   _bindCloseModal();
   _bindStepNavigation();
   _bindConfirm();
}

// ── Step UI ───────────────────────────────────────────────────────────────────
function _updateStepUI() {
   steps.forEach((s) => s.classList.remove("active"));
   stepperItems.forEach((s) => s.classList.remove("active"));
   document.getElementById(`step-${currentStep}`)?.classList.add("active");
   document.querySelector(`.step[data-step="${currentStep}"]`)?.classList.add("active");
}

function _resetModal() {
   currentStep = 1;
   _updateStepUI();
}

// ── Open / close modal ────────────────────────────────────────────────────────
function _bindOpenModal() {
   bookBtnSidebar.onclick = async () => {
      if (!startDate || !endDate) {
         alert("Please select a date range first.");
         return;
      }

      modal.style.display = "block";

      const diffDays = Math.ceil(Math.abs(endDate - startDate) / 864e5);
      if (displayDuration)  displayDuration.textContent  = diffDays * 24;
      if (displayBasePrice) displayBasePrice.textContent = (diffDays * pricePerDayRef.value).toLocaleString();

      await _prefillUserData();
      await _loadDynamicInsurances();
   };
}

function _bindCloseModal() {
   closeBtn.onclick = () => { modal.style.display = "none"; _resetModal(); };
   window.addEventListener("click", (e) => {
      if (e.target === modal) { modal.style.display = "none"; _resetModal(); }
   });
}

// ── Step navigation ───────────────────────────────────────────────────────────
function _bindStepNavigation() {
   nextBtns.forEach((btn) => {
      btn.onclick = () => {
         if (currentStep === 2 && !_validateStep2()) return;
         if (currentStep < 3) { currentStep++; _updateStepUI(); }
      };
   });

   prevBtns.forEach((btn) => {
      btn.onclick = () => {
         if (currentStep > 1) { currentStep--; _updateStepUI(); }
      };
   });
}

// ── Form validation ───────────────────────────────────────────────────────────
function _validateStep2() {
   let valid = true;

   [custFirstName, custLastName, custLicense].forEach((el) => {
      if (!el.value) { el.style.borderColor = "red"; valid = false; }
      else el.style.borderColor = "";
   });

   if (!custEmail.value || !custEmail.value.includes("@")) {
      custEmail.style.borderColor = "red"; valid = false;
   } else custEmail.style.borderColor = "";

   const phoneRegex = /^\+?[0-9\s-]{9,}$/;
   if (!phoneRegex.test(custPhone.value)) {
      custPhone.style.borderColor = "red";
      phoneError.style.display    = "block";
      valid = false;
   } else {
      custPhone.style.borderColor = "";
      phoneError.style.display    = "none";
   }

   return valid;
}

// ── Extras & price ────────────────────────────────────────────────────────────
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

async function _loadDynamicInsurances() {
   const container = document.getElementById("extras-grid-container");
   if (!container) return;

   container.innerHTML = `
      <div style="width: 100%; text-align: center; padding: 2rem;">
         <div class="spinner spinner--sm" style="margin: 0 auto 1rem;"></div>
         <span style="font-size: 1.4rem; color: #8d8a7c;">Loading extras…</span>
      </div>`;

   try {
      const insurances = await InsuranceService.getAll();
      const activeInsurances = insurances.filter(i => i.isActive);

      if (activeInsurances.length === 0) {
         container.innerHTML = `<p style="width: 100%; text-align: center; color: #8d8a7c; font-size: 1.5rem;">No extra additions available.</p>`;
         _initSliderButtons(0);
         _updatePrice();
         return;
      }

      container.innerHTML = activeInsurances.map((ins) => {
         const metadata = INSURANCE_METADATA[ins.name] || {
            icon: "shield-outline",
            desc: "Optional addition for your rental."
         };

         const priceLabel = ins.pricePerDay === 0 ? "Included" : `+${ins.pricePerDay} PLN / day`;
         const isActive = ins.pricePerDay === 0 ? "active" : "";

         return `
            <div class="extra-card ${isActive}" data-price="${ins.pricePerDay}" data-id="${ins.id}">
               <div class="extra-icon">
                  <ion-icon name="${metadata.icon}"></ion-icon>
               </div>
               <h4>${ins.name}</h4>
               <p>${metadata.desc}</p>
               <span class="extra-price">${priceLabel}</span>
            </div>
         `;
      }).join("");

      const cards = container.querySelectorAll(".extra-card");
      cards.forEach((card) => {
         card.onclick = () => {
            card.classList.toggle("active");
            _updatePrice();
         };
      });

      _initSliderButtons(activeInsurances.length);
      _updatePrice();

   } catch (error) {
      console.error("Error loading insurances:", error);
      container.innerHTML = `<p style="width: 100%; text-align: center; color: #e74c3c; font-size: 1.4rem;">Failed to load extras: ${error.message}</p>`;
   }
}

function _initSliderButtons(count) {
   const prevBtn = document.getElementById("extras-slide-prev");
   const nextBtn = document.getElementById("extras-slide-next");
   const container = document.getElementById("extras-grid-container");
   
   if (!prevBtn || !nextBtn || !container) return;

   if (count > 2) {
      prevBtn.style.display = "flex";
      nextBtn.style.display = "flex";

      prevBtn.onclick = () => {
         container.scrollBy({ left: -container.clientWidth / 2, behavior: "smooth" });
      };
      nextBtn.onclick = () => {
         container.scrollBy({ left: container.clientWidth / 2, behavior: "smooth" });
      };
   } else {
      prevBtn.style.display = "none";
      nextBtn.style.display = "none";
   }
}

function _updatePrice() {
   if (!startDate || !endDate) return;
   const diffDays = Math.ceil(Math.abs(endDate - startDate) / 864e5);
   const base     = diffDays * pricePerDayRef.value;

   let extras = 0;
   const cards = document.querySelectorAll(".extra-card");
   cards.forEach((c) => {
      if (c.classList.contains("active")) extras += parseFloat(c.dataset.price) * diffDays;
   });

   if (finalPriceDisplay) finalPriceDisplay.textContent = `${(base + extras).toLocaleString()} PLN`;
}

// ── User data prefill ─────────────────────────────────────────────────────────
async function _prefillUserData() {
   try {
      const profile = await UserService.getMyProfile();
      if (profile) {
         if (custFirstName) custFirstName.value = profile.firstName     || "";
         if (custLastName)  custLastName.value  = profile.lastName      || "";
         if (custEmail)     custEmail.value     = profile.email         || "";
         if (custPhone)     custPhone.value     = profile.phoneNumber   || "";
         if (custLicense)   custLicense.value   = profile.licenseNumber || "";
      } else {
         _fallbackEmail();
      }
   } catch {
      _fallbackEmail();
   }
}

function _fallbackEmail() {
   const email = localStorage.getItem("userEmail");
   if (email && custEmail) custEmail.value = email;
}

// ── Confirm & payment ─────────────────────────────────────────────────────────
function _bindConfirm() {
   confirmBookingFinal.onclick = async () => {
      const insuranceIds = [];
      const activeCards = document.querySelectorAll(".extra-card.active");
      activeCards.forEach((c) => {
         insuranceIds.push(parseInt(c.dataset.id));
      });

      const bookingData = {
         vehicleId:            vehicleId,
         customerEmail:        custEmail.value,
         firstName:            custFirstName.value,
         lastName:             custLastName.value,
         phoneNumber:          custPhone.value,
         licenseNumber:        custLicense.value,
         startDate:            startDate.toISOString(),
         endDate:              endDate.toISOString(),
         selectedInsuranceIds: insuranceIds,
      };

      try {
         confirmBookingFinal.disabled    = true;
         confirmBookingFinal.textContent = "Creating booking...";

         const response = await RentalService.createRental(bookingData);
         const rentalId = response.data?.id || response.id;

         if (!rentalId) throw new Error("Failed to retrieve rental ID from server response.");

         confirmBookingFinal.textContent = "Processing Payment...";

         const paymentResponse = await PaymentService.processPayment(rentalId, "MockCard");

         if (paymentResponse.success) {
            document.getElementById("success-tx-id").textContent = paymentResponse.transactionId;
            currentStep = 4;
            _updateStepUI();
         } else {
            alert("Booking created, but payment failed: " + paymentResponse.message);
            confirmBookingFinal.disabled    = false;
            confirmBookingFinal.textContent = "Try Payment Again";
         }
      } catch (error) {
         alert("Error: " + error.message);
         confirmBookingFinal.disabled    = false;
         confirmBookingFinal.textContent = "Confirm Booking";
      }
   };
}