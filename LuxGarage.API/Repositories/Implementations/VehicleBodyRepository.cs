using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing vehicle body data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle body information from the database.
/// </summary>
public class VehicleBodyRepository : IVehicleBodyRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the VehicleBodyRepository class, providing the necessary context for accessing the database
    /// and performing operations on vehicle body data.
    /// </summary>
    /// <param name="context">The database context for accessing vehicle body data.</param>
    public VehicleBodyRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a vehicle body by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle body to retrieve.</param>
    /// <returns>The vehicle body with the specified identifier, or null if not found.</returns>
    public async Task<VehicleBody?> GetByIdAsync(int id) 
        => await _context.VehicleBodies.FindAsync(id);
    
    /// <summary>
    /// Adds a new vehicle body to the database.
    /// </summary>
    /// <param name="body">The vehicle body to add.</param>
    public async Task AddAsync(VehicleBody body)
    {
        await _context.VehicleBodies.AddAsync(body);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing vehicle body in the database.
    /// </summary>
    /// <param name="body">The updated vehicle body information.</param>
    /// <param name="id">The unique identifier of the vehicle body to update.</param>
    public async Task UpdateAsync(VehicleBody body, int id)
    {
        VehicleBody? oldBody = await _context.VehicleBodies.FindAsync(id);

        oldBody = body;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a vehicle body from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle body to delete.</param>
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