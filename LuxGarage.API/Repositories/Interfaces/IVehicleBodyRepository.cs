using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle body data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting vehicle body information from the database.
/// </summary>
public interface IVehicleBodyRepository
{
    Task<VehicleBody?> GetByIdAsync(int id);
    Task AddAsync(VehicleBody body);
    Task UpdateAsync(VehicleBody body, int id);
    Task DeleteAsync(int id);
}
