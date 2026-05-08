using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages permission data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting permission information from the database.
/// </summary>
public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task AddAsync(Permission permission);
    Task UpdateAsync(Permission permission, int id);
    Task DeleteAsync(int id);
}