import "../layout.js";
import { initOfferPage, pricePerDay } from "./offerPage.js";
import { initCalendar }               from "./offerCalendar.js";
import { initBooking }                from "./offerBooking.js";

const urlParams = new URLSearchParams(window.location.search);
const offerId   = parseInt(urlParams.get("id")) || 1;

initOfferPage(offerId, () => {
   initCalendar(pricePerDay);
   initBooking();
});