import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", () => {
   const loginForm = document.getElementById("login-form");

   loginForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const loginRequest = {
         email: document.getElementById("email").value,
         password: document.getElementById("password").value
      }

      try {
         await AuthService.login(loginRequest);
         alert("Login successful");
         window.location.href = "index.html";
      } catch (error) {
         alert(error.message);
      }
   });
});