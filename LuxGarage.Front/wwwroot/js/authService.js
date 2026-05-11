const API_BASE_URL = "http://localhost:5054/api/Auth";

export const AuthService = {
   async login(login, password) {
      const response = await fetch(`${API_BASE_URL}/login`, {
         method: "POST",
         headers: { "Content-Type": "application/json" },
         body: JSON.stringify({ login, password }),
      });

      const result = await response.json();
      if (!response.ok) throw new Error(result.message || "Login error");

      localStorage.setItem("token", result.data.token);
      localStorage.setItem("user", JSON.stringify(result.data));
      return result.data;
   },

   async register(fullName, login, password) {
      const response = await fetch(`${API_BASE_URL}/register`, {
         method: "POST",
         headers: { "Content-Type": "application/json" },
         body: JSON.stringify({ fullName, login, password }),
      });

      const result = await response.json();
      if (!response.ok) throw new Error(result.message || "Register error");
      return result.data;
   },

   logout() {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "login.html";
   },

   getToken() {
      return localStorage.getItem("token");
   },
};
