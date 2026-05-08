using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages workplace data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting workplace information from the database.
/// </summary>
public interface IWorkplaceRepository
{
    Task<IEnumerable<Workplace>> GetAllAsync();
    Task<Workplace?> GetByIdAsync(int id);
    Task AddAsync(Workplace workplace);
    Task UpdateAsync(Workplace workplace, int id);
    Task DeleteAsync(int id);
}