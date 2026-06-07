using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Services.Interfaces;

public interface IRentalService
{
    Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end);
    Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request, int employeeId);
    Task<decimal> CalculateTotalPriceAsync(int vehicleId, DateTime start, DateTime end);

}