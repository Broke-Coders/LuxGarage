
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing vehicle brand data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle brand information from the database.
/// </summary>
public class VehicleBrandRepository : IVehicleBrandRepository
{
    private readonly RentalContext _context;
    
    /// <summary>
    /// Initializes a new instance of the VehicleBrandRepository class, providing the necessary context for accessing the database
    /// and performing operations on vehicle brand data.
    /// </summary>
    /// <param name="context">The database context for accessing vehicle brand data.</param>
    public VehicleBrandRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a vehicle brand by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle brand to retrieve.</param>
    /// <returns>The vehicle brand with the specified identifier, or null if not found.</returns>
    public async Task<VehicleBrand?> GetByIdAsync(int id) 
        => await _context.VehicleBrands.FindAsync(id);
    
    /// <summary>
    /// Adds a new vehicle brand to the database.
    /// </summary>
    /// <param name="brand">The vehicle brand to add.</param>
    public async Task AddAsync(VehicleBrand brand)
    {
        await _context.VehicleBrands.AddAsync(brand);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing vehicle brand in the database.
    /// </summary>
    /// <param name="brand">The updated vehicle brand information.</param>
    /// <param name="id">The unique identifier of the vehicle brand to update.</param>
    public async Task UpdateAsync(VehicleBrand brand, int id)
    {
        VehicleBrand? oldBrand = await _context.VehicleBrands.FindAsync(id);

        oldBrand = brand;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a vehicle brand from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle brand to delete.</param>
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