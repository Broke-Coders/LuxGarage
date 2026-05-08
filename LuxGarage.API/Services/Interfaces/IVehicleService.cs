using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;

namespace LuxGarage.API.Services.Interfaces;

/// <summary>
/// Defines the interface for the vehicle service in the LuxGarage API, providing methods for managing vehicle data, 
/// including retrieval, creation, and sorting of vehicles, while ensuring proper validation 
/// for vehicle data and returning appropriate responses for each action.
/// </summary>
public interface IVehicleService
{
    Task<List<VehicleListItemResponse>> GetAllAsync(GetVehiclesRequest request);
    Task<VehicleDetailsResponse?> GetByIdAsync(int id);
    Task<VehicleDetailsResponse> CreateAsync(CreateVehicleRequest request);
}