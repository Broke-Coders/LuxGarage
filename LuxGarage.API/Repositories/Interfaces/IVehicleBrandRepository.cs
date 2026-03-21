using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IVehicleBrandRepository
{
    Task<VehicleBrand?> GetByIdAsync(int id);
    Task AddAsync(VehicleBrand brand);
    Task UpdateAsync(VehicleBrand brand, int id);
    Task DeleteAsync(int id);
}
