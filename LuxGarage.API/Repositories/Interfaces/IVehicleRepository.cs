using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle information from the database.
/// </summary>
public interface IVehicleRepository
{
    /// <summary>
    /// Retrieves an IQueryable of all vehicles, enabling dynamic filtering and sorting at the database level.
    /// </summary>
    /// <returns>An IQueryable of vehicle records.</returns>
    IQueryable<Vehicle> GetAllQueryable();

    /// <summary>
    /// Retrieves a vehicle by its id from the database, including its associated brand, model, body, and color information.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to retrieve.</param>
    /// <returns>The vehicle with the specified identifier, or null if not found.</returns>
    Task<Vehicle?> GetByIdAsync(int id);
    
    /// <summary>
    /// Retrieves a vehicle by its license plate from the database.
    /// </summary>
    /// <param name="licensePlate">The license plate of the vehicle to retrieve.</param>
    /// <returns>The vehicle with the specified license plate, or null if not found.</returns>
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);

    /// <summary>
    /// Adds a new vehicle to the database.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle, int id);
    Task DeleteAsync(int id);
}