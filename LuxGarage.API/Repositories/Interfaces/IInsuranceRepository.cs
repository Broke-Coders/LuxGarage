using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages insurance data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting insurance information from the database.
/// </summary>
public interface IInsuranceRepository
{
    Task<Insurance?> GetByIdAsync(int id);
    Task AddAsync(Insurance insurance);
    Task UpdateAsync(Insurance insurance, int id);
    Task DeleteAsync(int id);
}