using LuxGarage.API.DTOs.Requests;

namespace LuxGarage.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
