import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", () => {
   const registerForm = document.getElementById("register-form");
   const isEmployeeCheckbox = document.getElementById("is-employee");
   const customerFields = document.getElementById("customer-fields");
   const employeeFields = document.getElementById("employee-fields");

   isEmployeeCheckbox.addEventListener("change", (e) => {
      if (e.target.checked) {
         customerFields.style.display = "none";
         employeeFields.style.display = "block";
      } else {
         customerFields.style.display = "block";
         employeeFields.style.display = "none";
      }
   });

   registerForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const password = document.getElementById("password").value;
      const confirmPassword = document.getElementById("confirm-password").value;

      if (password !== confirmPassword) {
         alert("Passwords do not match");
         return;
      }

      const isEmployee = isEmployeeCheckbox.checked;

      const registerRequest = {
         email: document.getElementById("email").value,
         password: password,
         firstName: document.getElementById("first-name").value,
         lastName: document.getElementById("last-name").value,
         isEmployee: isEmployee,
         phoneNumber: isEmployee ? null : document.getElementById("phone-number").value,
         licenseNumber: isEmployee ? null : document.getElementById("license-number").value,
         workplaceId: isEmployee ? parseInt(document.getElementById("workplace-id").value) : null
      };

      try {
         await AuthService.register(registerRequest);
         alert("Registration successful");
         window.location.href = "login.html";
      } catch (error) {
         alert(error.message);
      }
   });
});