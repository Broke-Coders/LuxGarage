using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IWorkplaceRepository
{
    Task<Workplace?> GetByIdAsync(int id);
    Task AddAsync(Workplace workplace);
    Task UpdateAsync(Workplace workplace, int id);
    Task DeleteAsync(int id);
}