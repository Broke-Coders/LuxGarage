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
}