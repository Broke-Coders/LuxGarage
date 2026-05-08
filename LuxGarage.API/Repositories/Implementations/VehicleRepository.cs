using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing vehicle data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting vehicle information from the database.
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the VehicleRepository class, providing the necessary context for accessing the database
    /// and performing operations on vehicle data.
    /// </summary>
    /// <param name="context">The database context for accessing vehicle data.</param>
    public VehicleRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all vehicles from the database, including their associated brand, model, body, and color information.
    /// </summary>
    /// <returns>A list of all vehicles in the database.</returns>
    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).AsNoTracking().ToListAsync();
    }
    //=> await _context.Vehicles.AsNoTracking().ToListAsync();


    /// <summary>
    /// Retrieves a vehicle by its id from the database, including its associated brand, model, body, and color information.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to retrieve.</param>
    /// <returns>The vehicle with the specified identifier, or null if not found.</returns>
    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).FirstOrDefaultAsync(v => v.Id == id);
    } //=> await _context.Vehicles.FindAsync(id);

    /// <summary>
    /// Retrieves a vehicle by its license plate from the database, including its associated brand, model, body, and color information.
    /// </summary>
    /// <param name="licensePlate">The license plate of the vehicle to retrieve.</param>
    /// <returns>The vehicle with the specified license plate, or null if not found.</returns>
    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleBrand)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleBody)
            .Include(v => v.VehicleColor).FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
    }
    //=> await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

    /// <summary>
    /// Adds a new vehicle to the database.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing vehicle in the database.
    /// </summary>
    /// <param name="vehicle">The updated vehicle information.</param>
    /// <param name="id">The unique identifier of the vehicle to update.</param>
    public async Task UpdateAsync(Vehicle vehicle, int id)
    {
        Vehicle? myVehicle = await _context.Vehicles.FindAsync(id);

        if (myVehicle == null)
            return;

        myVehicle.VehicleBrand = vehicle.VehicleBrand;
        myVehicle.VehicleBody = vehicle.VehicleBody;
        myVehicle.LicensePlate = vehicle.LicensePlate;
        myVehicle.Mileage = vehicle.Mileage;
        myVehicle.Horsepower = vehicle.Horsepower;
        myVehicle.VehicleColor = vehicle.VehicleColor;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a vehicle from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);
        if (vehicle is null) return;
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}