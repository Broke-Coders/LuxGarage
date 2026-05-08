using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

    /// <summary>
    /// Defines the contract for a repository that manages vehicle price data in the LuxGarage API,
    /// providing methods for retrieving, adding, updating, and deleting vehicle price information from the database.
    /// </summary>
    public interface IVehiclePriceRepository
    {
        Task<VehiclePrice?> GetByIdAsync(int id);
        Task AddAsync(VehiclePrice price);
        Task UpdateAsync(VehiclePrice price, int id);
        Task DeleteAsync(int id);
    }
