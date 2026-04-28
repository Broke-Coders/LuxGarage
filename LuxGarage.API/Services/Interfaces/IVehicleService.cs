using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;

namespace LuxGarage.API.Services.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleListItemResponse>> GetAllAsync(GetVehiclesRequest request);
    Task<VehicleDetailsResponse?> GetByIdAsync(int id);
    Task<VehicleDetailsResponse> CreateAsync(CreateVehicleRequest request);
}