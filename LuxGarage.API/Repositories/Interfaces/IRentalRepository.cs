using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(int id);
    Task AddAsync(Rental rental);
    Task UpdateAsync(Rental rental, int id);
    Task DeleteAsync(int id);
}