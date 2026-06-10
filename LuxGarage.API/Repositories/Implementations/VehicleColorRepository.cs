using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleColorRepository : IVehicleColorRepository
{
    private readonly RentalContext _context;

    public VehicleColorRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<VehicleColor?> GetByIdAsync(int id) 
        => await _context.VehicleColors.FindAsync(id);
    
    public async Task AddAsync(VehicleColor color)
    {
        await _context.VehicleColors.AddAsync(color);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VehicleColor color, int id)
    {
        VehicleColor? oldColor = await _context.VehicleColors.FindAsync(id);

        oldColor = color;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var color = await _context.VehicleColors.FindAsync(id);
        
        if (color == null)
        {
            Console.WriteLine("Vehicle color with given id not found");
            return;
        }

        _context.VehicleColors.Remove(color);
        await _context.SaveChangesAsync();
    }
}