using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle color data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle color information from the database.
/// </summary>
public interface IVehicleColorRepository
{
    Task<VehicleColor?> GetByIdAsync(int id);
    Task AddAsync(VehicleColor color);
    Task UpdateAsync(VehicleColor color, int id);
    Task DeleteAsync(int id);
}
