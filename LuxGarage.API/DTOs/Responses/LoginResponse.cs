namespace LuxGarage.API.DTOs.Responses
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string Login { get; set; }
        public required string Role { get; set; }
    }
}
