// js/offerCalendar.js
import { bookedDates } from "./offerPage.js";

const MONTHS = [
   "January", "February", "March", "April", "May", "June",
   "July", "August", "September", "October", "November", "December",
];
const currentYear = new Date().getFullYear();
const YEARS = Array.from({ length: 6 }, (_, i) => currentYear + i);

// DOM refs
const monthYearDisplay = document.getElementById("month-year-display");
const selectorsOverlay  = document.getElementById("selectors-overlay");
const monthList         = document.getElementById("month-list");
const yearList          = document.getElementById("year-list");
const calendarGrid      = document.querySelector(".calendar-grid");
const summaryDays       = document.getElementById("summary-days");
const summaryRange      = document.getElementById("summary-range");
const summaryPriceValue = document.getElementById("summary-price-value");
const totalPriceBottom  = document.getElementById("total-price-bottom");
const bookBtnSidebar    = document.querySelector(".btn-book-sidebar");
const availabilityError = document.getElementById("availability-error");
const timeButtons       = document.querySelectorAll(".time-btn");

const prevBtn = document.querySelector(".cal-nav ion-icon[name='chevron-back-outline']");
const nextBtn = document.querySelector(".cal-nav ion-icon[name='chevron-forward-outline']");

// State
let currentDate = new Date();
currentDate.setDate(1);
export let startDate      = null;
export let endDate        = null;
export let startTime      = "12:00 PM";
export let pricePerDayRef = { value: 0 };

// ── Public init ──────────────────────────────────────────────────────────────
export function initCalendar(pricePerDay) {
   pricePerDayRef.value = pricePerDay;
   renderCalendar();
   updateDisplay();
   _bindNavigation();
   _bindTimePicker();
   _bindSelectorOverlay();
}

// ── Calendar render ──────────────────────────────────────────────────────────
export function renderCalendar() {
   const year  = currentDate.getFullYear();
   const month = currentDate.getMonth();
   const today = new Date();
   today.setHours(0, 0, 0, 0);

   if (monthYearDisplay) {
      monthYearDisplay.textContent = `${MONTHS[month]} ${year}`;
   }

   const labels = Array.from(calendarGrid.querySelectorAll(".day-label"));
   calendarGrid.innerHTML = "";
   labels.forEach((l) => calendarGrid.appendChild(l));

   const firstDay      = new Date(year, month, 1).getDay();
   const daysInMonth   = new Date(year, month + 1, 0).getDate();
   const daysInPrevMon = new Date(year, month, 0).getDate();

   // Prev-month filler
   for (let i = firstDay; i > 0; i--) {
      const d = document.createElement("div");
      d.classList.add("day", "disabled");
      d.textContent = daysInPrevMon - i + 1;
      calendarGrid.appendChild(d);
   }

   // Current month days
   for (let i = 1; i <= daysInMonth; i++) {
      const date = new Date(year, month, i);
      date.setHours(0, 0, 0, 0);

      const d = document.createElement("div");
      d.classList.add("day");
      d.textContent = i;

      if (date < today) d.classList.add("past-day");

      const isBooked = bookedDates.some((r) => date >= r.start && date <= r.end);
      if (isBooked && date >= today) d.classList.add("unavailable-day");

      if (startDate && date.getTime() === startDate.getTime()) {
         d.classList.add("selected-start");
      } else if (endDate && date.getTime() === endDate.getTime()) {
         d.classList.add("selected-end");
      } else if (startDate && endDate && date > startDate && date < endDate) {
         d.classList.add("in-range");
      }

      d.onclick = () => _handleDayClick(date, d);
      calendarGrid.appendChild(d);
   }

   // Next-month filler (fill to 42 cells)
   const extra = 42 - (firstDay + daysInMonth);
   for (let i = 1; i <= extra; i++) {
      const d = document.createElement("div");
      d.classList.add("day", "disabled");
      d.textContent = i;
      calendarGrid.appendChild(d);
   }
}

// ── Display update ────────────────────────────────────────────────────────────
export function updateDisplay() {
   const price = pricePerDayRef.value;

   if (startDate && endDate) {
      const diffDays = Math.ceil(Math.abs(endDate - startDate) / 864e5);
      summaryDays.textContent = diffDays;

      const opts = { month: "short", day: "numeric" };
      summaryRange.textContent =
         `${startDate.toLocaleDateString("en-US", opts)} - ` +
         `${endDate.toLocaleDateString("en-US", opts)}, ${startDate.getFullYear()}`;

      const total     = diffDays * price;
      const formatted = `${total.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })} PLN`;
      summaryPriceValue.textContent = formatted;
      totalPriceBottom.textContent  = formatted;

      const available = _isRangeAvailable(startDate, endDate);
      bookBtnSidebar.classList.toggle("ready", available);
      bookBtnSidebar.disabled = !available;
      availabilityError.classList.toggle("active", !available);
      bookBtnSidebar.textContent = available ? "Confirm Booking" : "Range Unavailable";

   } else if (startDate) {
      summaryDays.textContent = "0";
      summaryRange.textContent = `From ${startDate.toLocaleDateString("en-US", { month: "short", day: "numeric" })}`;
      summaryPriceValue.textContent = "0.00 PLN";
      totalPriceBottom.textContent  = "0.00 PLN";
      bookBtnSidebar.classList.remove("ready");
      bookBtnSidebar.disabled = false;
      availabilityError.classList.remove("active");

   } else {
      summaryDays.textContent       = "0";
      summaryRange.textContent      = "Select dates in calendar";
      summaryPriceValue.textContent = "0.00 PLN";
      totalPriceBottom.textContent  = "0.00 PLN";
      bookBtnSidebar.classList.remove("ready");
      bookBtnSidebar.disabled = false;
      availabilityError.classList.remove("active");
   }
}

// ── Private helpers ───────────────────────────────────────────────────────────
function _handleDayClick(date, el) {
   if (el.classList.contains("unavailable-day") || el.classList.contains("past-day")) return;

   if (!startDate || (startDate && endDate)) {
      startDate = date;
      endDate   = null;
   } else if (startDate && !endDate) {
      if (date < startDate) {
         startDate = date;
      } else if (date.getTime() !== startDate.getTime()) {
         endDate = date;
      }
   }

   updateDisplay();
   renderCalendar();
}

function _isRangeAvailable(start, end) {
   if (!start || !end) return true;
   let temp = new Date(start);
   while (temp <= end) {
      if (bookedDates.some((r) => temp >= r.start && temp <= r.end)) return false;
      temp.setDate(temp.getDate() + 1);
   }
   return true;
}

function _bindNavigation() {
   if (prevBtn) {
      prevBtn.onclick = () => {
         currentDate.setMonth(currentDate.getMonth() - 1);
         renderCalendar();
      };
   }
   if (nextBtn) {
      nextBtn.onclick = () => {
         currentDate.setMonth(currentDate.getMonth() + 1);
         renderCalendar();
      };
   }
}

function _bindTimePicker() {
   timeButtons[3]?.classList.add("active");
   timeButtons.forEach((btn) => {
      btn.onclick = () => {
         timeButtons.forEach((b) => b.classList.remove("active"));
         btn.classList.add("active");
         startTime = btn.textContent;
      };
   });
}

function _bindSelectorOverlay() {
   if (!monthYearDisplay) return;

   monthYearDisplay.onclick = (e) => {
      e.stopPropagation();
      _populateSelectors();
      selectorsOverlay.classList.toggle("active");
   };

   document.addEventListener("click", (e) => {
      if (selectorsOverlay &&
          !selectorsOverlay.contains(e.target) &&
          e.target !== monthYearDisplay) {
         selectorsOverlay.classList.remove("active");
      }
   });
}

function _populateSelectors() {
   monthList.innerHTML = "";
   MONTHS.forEach((name, idx) => {
      const item = document.createElement("div");
      item.classList.add("selector-item");
      item.textContent = name;
      if (idx === currentDate.getMonth()) item.classList.add("current");
      item.onclick = (e) => {
         e.stopPropagation();
         currentDate.setMonth(idx);
         selectorsOverlay.classList.remove("active");
         renderCalendar();
      };
      monthList.appendChild(item);
   });

   yearList.innerHTML = "";
   YEARS.forEach((year) => {
      const item = document.createElement("div");
      item.classList.add("selector-item");
      item.textContent = year;
      if (year === currentDate.getFullYear()) item.classList.add("current");
      item.onclick = (e) => {
         e.stopPropagation();
         currentDate.setFullYear(year);
         selectorsOverlay.classList.remove("active");
         renderCalendar();
      };
      yearList.appendChild(item);
   });
}