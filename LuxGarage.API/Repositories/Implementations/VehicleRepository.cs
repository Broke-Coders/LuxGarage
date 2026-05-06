using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    public VehicleRepository(RentalContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).AsNoTracking().ToListAsync();
    }
    //=> await _context.Vehicles.AsNoTracking().ToListAsync();


    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).FirstOrDefaultAsync(v => v.Id == id);
    } //=> await _context.Vehicles.FindAsync(id);

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
    }
    //=> await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

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