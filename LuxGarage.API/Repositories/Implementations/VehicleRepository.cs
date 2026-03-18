
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    public VehicleRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
        => await _context.Vehicles.AsNoTracking().ToListAsync();

    public async Task<Vehicle?> GetByIdAsync(int id)
        => await _context.Vehicles.FindAsync(id);
}