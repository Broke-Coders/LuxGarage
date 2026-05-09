using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IOfferRepository
{
    Task<List<Offer>> GetAllAsync();
    Task<Offer?> GetByIdAsync(int id);
    Task<Offer?> GetByVehicleIdAsync(int vehicleId);
    Task AddAsync(Offer offer);
    Task UpdateAsync(Offer offer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
