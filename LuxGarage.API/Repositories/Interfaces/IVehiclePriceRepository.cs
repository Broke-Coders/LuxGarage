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
        /// <summary>
        /// Retrieves the currently active daily rental price for a specific vehicle.
        /// Searches across associated offers and valid time frames to find the effective price.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID.</param>
        /// <returns>The current price per day.</returns>
        Task<decimal> GetCurrentPriceByVehicleIdAsync(int vehicleId);
    }
