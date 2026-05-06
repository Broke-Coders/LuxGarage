using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Implementations;
public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    public VehicleRepository(RentalContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
        => await _context.Vehicles.AsNoTracking().ToListAsync();

    public async Task<Vehicle?> GetByIdAsync(int id)
        => await _context.Vehicles.FindAsync(id);

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        => await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle, int id)
    {
        Vehicle? myVehicle = await _context.Vehicles.FindAsync(id);

        if (myVehicle == null)
            return;

        myVehicle.VehicleBrand = vehicle.VehicleBrand;
        myVehicle.VehicleBody = vehicle.VehicleBody;
        myVehicle.LicensePlate = vehicle.LicensePlate;
        myVehicle.Mileage = vehicle.Mileage;
        myVehicle.Horsepower = vehicle.Horsepower;
        myVehicle.VehicleColor = vehicle.VehicleColor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);
        if (vehicle is null) return;
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}