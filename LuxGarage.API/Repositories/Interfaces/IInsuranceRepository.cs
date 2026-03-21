using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IInsuranceRepository
{
    Task<Insurance?> GetByIdAsync(int id);
    Task AddAsync(Insurance borrower);
    Task UpdateAsync(Insurance borrower);
    Task DeleteAsync(int id);
}