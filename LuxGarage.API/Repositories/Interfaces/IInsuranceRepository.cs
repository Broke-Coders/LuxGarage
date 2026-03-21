using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IInsuranceRepository
{
    Task<Insurance?> GetByIdAsync(int id);
    Task AddAsync(Insurance insurance);
    Task UpdateAsync(Insurance insurance, int id);
    Task DeleteAsync(int id);
}