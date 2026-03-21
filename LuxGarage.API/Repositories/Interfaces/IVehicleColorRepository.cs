using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IVehicleColorRepository
{
    Task<VehicleColor?> GetByIdAsync(int id);
    Task AddAsync(VehicleColor color);
    Task UpdateAsync(VehicleColor color, int id);
    Task DeleteAsync(int id);
}
