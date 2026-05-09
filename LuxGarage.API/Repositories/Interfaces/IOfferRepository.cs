using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages offer data in the LuxGarage API.
/// </summary>
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
