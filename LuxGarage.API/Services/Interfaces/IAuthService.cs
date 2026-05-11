using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;

namespace LuxGarage.API.Services.Interfaces;
   
/// <summary>
/// Defines the interface for the authentication service in the LuxGarage API, providing methods for user registration and login,
/// ensuring proper handling of authentication-related operations and returning appropriate responses for each action.
/// </summary>
public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);

}