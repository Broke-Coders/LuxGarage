using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task AddAsync(Permission permission);
    Task UpdateAsync(Permission permission, int id);
    Task DeleteAsync(int id);
}