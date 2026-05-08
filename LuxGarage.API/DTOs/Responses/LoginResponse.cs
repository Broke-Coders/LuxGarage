namespace LuxGarage.API.DTOs.Responses
{
    /// <summary>
    /// DTO for login response, containing the JWT token, user's login, and role to be returned in API responses 
    /// when a user successfully logs in to the system.
    /// </summary>
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string Login { get; set; }
        public required string Role { get; set; }
    }
}
