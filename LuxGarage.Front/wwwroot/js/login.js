import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", () => {
   const loginForm = document.getElementById("login-form");
   const messageBox = document.getElementById("form-message");

   const showMessage = (msg, isError = false) => {
      messageBox.textContent = msg;
      messageBox.className = "form-message " + (isError ? "Error" : "Success");
   };

   loginForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const loginRequest = {
         email: document.getElementById("email").value,
         password: document.getElementById("password").value
      }

      try {
         await AuthService.login(loginRequest);
         showMessage("Login successful! Redirecting...", false);
         setTimeout(() => {
            window.location.href = "index.html";
         }, 2000);
      } catch (error) {
         showMessage(error.message, true);
      }
   });
});