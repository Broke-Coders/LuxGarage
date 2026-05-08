namespace LuxGarage.API.DTOs.Requests
{
    /// <summary>
    /// DTO for user login, containing the user's login and password to be used for authentication and 
    /// obtaining a JWT token for authorized access to the API.
    /// </summary>
    public class LoginRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
