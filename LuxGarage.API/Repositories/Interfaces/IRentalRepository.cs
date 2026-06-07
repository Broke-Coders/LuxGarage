using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages rental data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting rental information from the database.
/// </summary>
public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(int id);
    Task AddAsync(Rental rental);
    Task UpdateAsync(Rental rental, int id);
    Task DeleteAsync(int id);
    /// <summary>
    /// Retrieves all rental records associated with a specific vehicle. 
    /// Primarily used for availability checking and historical analysis.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>A list of rentals for the specified vehicle.</returns>
    Task<List<Rental>> GetByVehicleIdAsync(int vehicleId);
}