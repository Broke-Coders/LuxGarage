import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", () => {
   const registerForm = document.querySelector(".auth-form");

   registerForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const fullName = document.getElementById("full-name").value;
      const login = document.getElementById("login").value;
      const password = document.getElementById("password").value;
      const confirmPassword = document.getElementById("confirm-password").value;

      if (password !== confirmPassword) {
         alert("Passwords do not match");
         return;
      }

      try {
         await AuthService.register(fullName, login, password);
         alert("Registration successful");
         window.location.href = "login.html";
      } catch (error) {
         alert(error.message);
      }
   });
});
