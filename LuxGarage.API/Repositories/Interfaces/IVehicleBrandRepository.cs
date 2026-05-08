using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle brand data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle brand information from the database.
/// </summary>
public interface IVehicleBrandRepository
{
    Task<VehicleBrand?> GetByIdAsync(int id);
    Task AddAsync(VehicleBrand brand);
    Task UpdateAsync(VehicleBrand brand, int id);
    Task DeleteAsync(int id);
}
