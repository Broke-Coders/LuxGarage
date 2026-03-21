
using LuxGarage.API.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;
using System.Collections.Immutable;

public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    public VehicleRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
        => (IEnumerable<Task>)await _context.Vehicles.AsNoTracking().ToListAsync();

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
        Vehicle? myvehicle = await _context.Vehicles.FindAsync(id);  

        if (myvehicle == null)
        {
            Console.WriteLine("Nie ma elementu o podanym id");
            return;        
        } 

        myvehicle = vehicle;

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