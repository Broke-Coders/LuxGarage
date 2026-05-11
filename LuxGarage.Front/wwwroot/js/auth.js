import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", () => {
   const loginForm = document.querySelector(".auth-form");

   loginForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const login = document.getElementById("login").value;
      const password = document.getElementById("password").value;

      try {
         await AuthService.login(login, password);
         alert("Login successful");
         window.location.href = "dashboard.html";
      } catch (error) {
         alert(error.message);
      }
   });
});
