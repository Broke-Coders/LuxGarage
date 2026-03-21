
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleBrandRepository : IVehicleBrandRepository
{
    private readonly RentalContext _context;

    public VehicleBrandRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<VehicleBrand?> GetByIdAsync(int id) 
        => await _context.VehicleBrands.FindAsync(id);
    
    public async Task AddAsync(VehicleBrand brand)
    {
        await _context.VehicleBrands.AddAsync(brand);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VehicleBrand brand, int id)
    {
        VehicleBrand? oldBrand = await _context.VehicleBrands.FindAsync(id);

        oldBrand = brand;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var brand = await _context.VehicleBrands.FindAsync(id);
        
        if (brand == null)
        {
            Console.WriteLine("Vehicle brand with given id not found");
            return;
        }

        _context.VehicleBrands.Remove(brand);
        await _context.SaveChangesAsync();
    }
}