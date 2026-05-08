
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing insurance data in the LuxGarage API, providing methods for retrieving, adding, updating,
/// and deleting insurance information from the database.
/// </summary>
public class InsuranceRepository : IInsuranceRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the InsuranceRepository class, providing the necessary context for accessing the database
    /// and performing operations on insurance data.    
    /// </summary>
    /// <param name="context">The database context for accessing insurance data.</param>
    public InsuranceRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves an insurance by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the insurance to retrieve.</param>
    /// <returns>The insurance with the specified identifier, or null if not found.</returns>
    public async Task<Insurance?> GetByIdAsync(int id) 
        => await _context.Insurances.FindAsync(id);
    
    /// <summary>
    /// Adds a new insurance to the database.
    /// </summary>
    /// <param name="insurance">The insurance to add.</param>
    public async Task AddAsync(Insurance insurance)
    {
        await _context.Insurances.AddAsync(insurance);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing insurance in the database.
    /// </summary>
    /// <param name="insurance">The updated insurance information.</param>
    /// <param name="id">The unique identifier of the insurance to update.</param>
    public async Task UpdateAsync(Insurance insurance, int id)
    {
        Insurance? oldInsurance = await _context.Insurances.FindAsync(id);

        oldInsurance = insurance;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an insurance from the database based on its unique identifier, allowing for the removal of insurance records from the system.
    /// </summary>
    /// <param name="id">The identifier of the insurance to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var insurance = await _context.Insurances.FindAsync(id);
        
        if (insurance == null)
        {
            Console.WriteLine("insurance with given id not found");
            return;
        }

        _context.Insurances.Remove(insurance);
        await _context.SaveChangesAsync();
    }
}