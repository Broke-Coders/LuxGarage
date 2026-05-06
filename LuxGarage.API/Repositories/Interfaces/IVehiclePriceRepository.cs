using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

    public interface IVehiclePriceRepository
    {
        Task<VehiclePrice?> GetByIdAsync(int id);
        Task AddAsync(VehiclePrice price);
        Task UpdateAsync(VehiclePrice price, int id);
        Task DeleteAsync(int id);
    }
