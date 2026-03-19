
using LuxGarage.API.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;

public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    public VehicleRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
        => await _context.Vehicles.AsNoTracking().ToListAsync();

    public async Task<Vehicle?> GetByIdAsync(int id)
        => await _context.Vehicles.FindAsync(id);

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
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