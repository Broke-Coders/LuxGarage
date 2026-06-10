using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IVehicleBodyRepository
{
    Task<VehicleBody?> GetByIdAsync(int id);
    Task AddAsync(VehicleBody body);
    Task UpdateAsync(VehicleBody body, int id);
    Task DeleteAsync(int id);
}
