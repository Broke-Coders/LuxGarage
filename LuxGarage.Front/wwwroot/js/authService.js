const API_BASE_URL = "http://localhost:5054/api/Auth";

export const AuthService = {
   async login(loginRequest) {
      const response = await fetch(`${API_BASE_URL}/login`, {
         method: "POST",
         headers: { "Content-Type": "application/json" },
         body: JSON.stringify(loginRequest),
      });

      if (!response.ok) {
         const errorInfo = await response.json();
         throw new Error(errorInfo.message || "Login error");
      }

      const result = await response.json();

      localStorage.setItem("jwtToken", result.token);
      localStorage.setItem("userRole", result.role);
      localStorage.setItem("userEmail", result.email);
      return result;
   },

   async register(registerRequest) {
      const response = await fetch(`${API_BASE_URL}/register`, {
         method: "POST",
         headers: { "Content-Type": "application/json" },
         body: JSON.stringify(registerRequest),
      });

      if (!response.ok) {
         const errorInfo = await response.json();
         throw new Error(errorInfo.message || "Register error");
      } 

      return result.data;
   },

   logout() {
      localStorage.setItem("jwtToken", result.token);
      localStorage.setItem("userRole", result.role);
      localStorage.setItem("userEmail", result.email);
      window.location.href = "login.html";
   },

   getToken() {
      return localStorage.getItem("jwtToken");
   },
};
