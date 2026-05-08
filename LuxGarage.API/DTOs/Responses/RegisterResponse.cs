using System.Reflection.Metadata.Ecma335;

namespace LuxGarage.API.DTOs.Responses
{
    /// <summary>
    /// DTO for user registration response, containing the newly registered user's ID and login to be returned in API 
    /// responses when a new user account is successfully created in the system.
    /// </summary>
    public class RegisterResponse
    {
        public int Id { get; set; }
        public string Login { get; set; }
    }
}
