import { AuthService } from "./authService.js";

document.addEventListener("DOMContentLoaded", async () => {
   const registerForm = document.getElementById("register-form");
   const isEmployeeCheckbox = document.getElementById("is-employee");
   const customerFields = document.getElementById("customer-fields");
   const employeeFields = document.getElementById("employee-fields");
   const messageBox = document.getElementById("form-message");
   const workplaceSelect = document.getElementById("workplace-id");

   const showMessage = (msg, isError = false) => {
      messageBox.textContent = msg;
      messageBox.className = "form-message " + (isError ? "error" : "success");
   };

   isEmployeeCheckbox.addEventListener("change", (e) => {
      if (e.target.checked) {
         customerFields.style.display = "none";
         employeeFields.style.display = "block";
      } else {
         customerFields.style.display = "block";
         employeeFields.style.display = "none";
      }
   });

   try {
      const wpResponse = await fetch("http://localhost:5054/api/Workplace");
      if (wpResponse.ok) {
         const apiResponse = await wpResponse.json();
         const workplaces = apiResponse.data || apiResponse.data || [];
         workplaces.forEach(wp => {
            const option = document.createElement("option");
            option.value = wp.id;
            option.textContent = `${wp.country}, ${wp.city}, ${wp.street}, ${wp.buildingNumber}`;
            workplaceSelect.appendChild(option);
         });
      } else {
      console.error("Error while loading LuxGarage Branches:", wpResponse.status);
      }
   } catch (error) {
      console.error("Error: cannot load LuxGarage Branches", error);
   }

   registerForm.addEventListener("submit", async (e) => {
      e.preventDefault();

      const password = document.getElementById("password").value;
      const confirmPassword = document.getElementById("confirm-password").value;

      if (password !== confirmPassword) {
         showMessage("Passwords do not match", true);
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
         workplaceId: isEmployee ? parseInt(workplaceSelect.value) : null
      };

      try {
         await AuthService.register(registerRequest);
         showMessage("Registration successful!", false);
         setTimeout(() => {
            window.location.href = "login.html";
         }, 2000);
      } catch (error) {
         showMessage(error.message, true);
      }
   });
});