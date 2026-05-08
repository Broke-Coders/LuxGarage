using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing vehicle color data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle color information from the database.
/// </summary>
public class VehicleColorRepository : IVehicleColorRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the VehicleColorRepository class, providing the necessary context for accessing the database
    /// and performing operations on vehicle color data.
    /// </summary>
    /// <param name="context">The database context for accessing vehicle color data.</param>
    public VehicleColorRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a vehicle color by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle color to retrieve.</param>
    /// <returns>The vehicle color with the specified identifier, or null if not found.</returns>
    public async Task<VehicleColor?> GetByIdAsync(int id) 
        => await _context.VehicleColors.FindAsync(id);
    
    /// <summary>
    /// Adds a new vehicle color to the database.
    /// </summary>
    /// <param name="color">The vehicle color to add.</param>
    public async Task AddAsync(VehicleColor color)
    {
        await _context.VehicleColors.AddAsync(color);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing vehicle color in the database.
    /// </summary>
    /// <param name="color">The updated vehicle color information.</param>
    /// <param name="id">The unique identifier of the vehicle color to update.</param>
    public async Task UpdateAsync(VehicleColor color, int id)
    {
        VehicleColor? oldColor = await _context.VehicleColors.FindAsync(id);

        oldColor = color;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a vehicle color from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle color to delete.</param>
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