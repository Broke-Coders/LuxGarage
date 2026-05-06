using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

    public class VehiclePriceRepository : IVehiclePriceRepository
{
    private readonly RentalContext _context;

    public VehiclePriceRepository(RentalContext context)
    {
        _context = context;
    }
    public async Task<VehiclePrice?> GetByIdAsync(int id) 
        => await _context.VehiclePrices.FindAsync(id);

    public async Task AddAsync(VehiclePrice vehiclePrice)
    {
        await _context.VehiclePrices.AddAsync(vehiclePrice);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(VehiclePrice vehiclePrice, int id)
    {
        VehiclePrice? oldVehiclePrice = await _context.VehiclePrices.FindAsync(id);

        oldVehiclePrice = vehiclePrice;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var vehiclePrice = await _context.VehiclePrices.FindAsync(id);
        
        if (vehiclePrice == null)
        {
            Console.WriteLine("vehicle price with given id not found");
            return;
        }

        _context.VehiclePrices.Remove(vehiclePrice);
        await _context.SaveChangesAsync();
    }
}
