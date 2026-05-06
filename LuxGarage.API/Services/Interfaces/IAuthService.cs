using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;

namespace LuxGarage.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequest request);

    }
}
