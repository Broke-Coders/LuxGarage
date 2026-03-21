using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IWorkerRepository
{
    Task<Worker?> GetByIdAsync(int id);
    Task AddAsync(Worker worker);
    Task UpdateAsync(Worker worker, int id);
    Task DeleteAsync(int id);
}