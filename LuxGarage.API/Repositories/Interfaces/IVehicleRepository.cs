using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle information from the database.
/// </summary>
public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle, int id);
    Task DeleteAsync(int id);
}