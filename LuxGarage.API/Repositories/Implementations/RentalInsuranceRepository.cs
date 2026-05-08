using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing rental insurance data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating,
/// and deleting rental insurance information from the database.
/// </summary>
public class RentalInsuranceRepository : IRentalInsuranceRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the RentalInsuranceRepository class, providing the necessary context for accessing the database
    /// and performing operations on rental insurance data.
    /// </summary>
    /// <param name="context">The database context for accessing rental insurance data.</param>
    public RentalInsuranceRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a rental insurance by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the rental insurance to retrieve.</param>
    /// <returns>The rental insurance with the specified identifier, or null if not found.</returns>
    public async Task<RentalInsurance?> GetByIdAsync(int id) 
        => await _context.RentalInsurances.FindAsync(id);
    

    /// <summary>
    /// Adds a new rental insurance to the database.
    /// </summary>
    /// <param name="rentalInsurance">The rental insurance to add.</param>
    public async Task AddAsync(RentalInsurance rentalInsurance)
    {
        await _context.RentalInsurances.AddAsync(rentalInsurance);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing rental insurance in the database.
    /// </summary>
    /// <param name="rentalInsurance">The updated rental insurance information.</param>
    /// <param name="id">The unique identifier of the rental insurance to update.</param>
    public async Task UpdateAsync(RentalInsurance rentalInsurance, int id)
    {
        RentalInsurance? oldRentalInsurance = await _context.RentalInsurances.FindAsync(id);

        oldRentalInsurance = rentalInsurance;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a rental insurance from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the rental insurance to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var rentalInsurance = await _context.RentalInsurances.FindAsync(id);
        
        if (rentalInsurance == null)
        {
            Console.WriteLine("rentalInsurance with given id not found");
            return;
        }

        _context.RentalInsurances.Remove(rentalInsurance);
        await _context.SaveChangesAsync();
    }
}