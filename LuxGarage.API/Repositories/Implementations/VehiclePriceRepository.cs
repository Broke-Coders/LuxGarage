using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

    /// <summary>
    /// Represents a repository for managing vehicle price data in the LuxGarage API,
    /// providing methods for retrieving, adding, updating, and deleting vehicle price information from the database
    /// </summary>
    public class VehiclePriceRepository : IVehiclePriceRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the VehiclePriceRepository class, providing the necessary context for accessing the database
    /// and performing operations on vehicle price data.
    /// </summary>
    /// <param name="context">The database context for accessing vehicle price data.</param>
    public VehiclePriceRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a vehicle price by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle price to retrieve.</param>
    /// <returns>The vehicle price with the specified identifier, or null if not found.</returns>
    public async Task<VehiclePrice?> GetByIdAsync(int id) 
        => await _context.VehiclePrices.FindAsync(id);

    /// <summary>
    /// Adds a new vehicle price to the database.
    /// </summary>
    /// <param name="vehiclePrice">The vehicle price to add.</param>
    public async Task AddAsync(VehiclePrice vehiclePrice)
    {
        await _context.VehiclePrices.AddAsync(vehiclePrice);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing vehicle price in the database.
    /// </summary>
    /// <param name="vehiclePrice">The updated vehicle price information.</param>
    /// <param name="id">The unique identifier of the vehicle price to update.</param>
    public async Task UpdateAsync(VehiclePrice vehiclePrice, int id)
    {
        VehiclePrice? oldVehiclePrice = await _context.VehiclePrices.FindAsync(id);

        oldVehiclePrice = vehiclePrice;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a vehicle price from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle price to delete.</param>
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