using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleBodyRepository : IVehicleBodyRepository
{
    private readonly RentalContext _context;

    public VehicleBodyRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<VehicleBody?> GetByIdAsync(int id) 
        => await _context.VehicleBodies.FindAsync(id);
    
    public async Task AddAsync(VehicleBody body)
    {
        await _context.VehicleBodies.AddAsync(body);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VehicleBody body, int id)
    {
        VehicleBody? oldBody = await _context.VehicleBodies.FindAsync(id);

        oldBody = body;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var body = await _context.VehicleBodies.FindAsync(id);
        
        if (body == null)
        {
            Console.WriteLine("Vehicle body with given id not found");
            return;
        }

        _context.VehicleBodies.Remove(body);
        await _context.SaveChangesAsync();
    }
}