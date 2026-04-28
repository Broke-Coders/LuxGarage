using LuxGarage.API.Models;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle, int id);
    Task DeleteAsync(int id);
}