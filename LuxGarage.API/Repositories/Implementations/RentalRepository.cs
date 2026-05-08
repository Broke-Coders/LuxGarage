using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing rental data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting rental records. 
/// </summary>
public class RentalRepository : IRentalRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the RentalRepository class, providing the necessary context for accessing the database
    /// and performing operations on rental data.
    /// </summary>
    /// <param name="context">The database context for accessing rental data.</param>
    public RentalRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a rental by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the rental to retrieve.</param>
    /// <returns>The rental with the specified identifier, or null if not found.</returns>
    public async Task<Rental?> GetByIdAsync(int id) 
        => await _context.Rentals.FindAsync(id);
    

    /// <summary>
    /// Adds a new rental to the database.
    /// </summary>
    /// <param name="rental">The rental to add.</param>
    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing rental in the database.
    /// </summary>
    /// <param name="rental">The updated rental information.</param>
    /// <param name="id">The unique identifier of the rental to update.</param>
    public async Task UpdateAsync(Rental rental, int id)
    {
        Rental? oldRental = await _context.Rentals.FindAsync(id);

        oldRental = rental;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a rental from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the rental to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        
        if (rental == null)
        {
            Console.WriteLine("rental with given id not found");
            return;
        }

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
    }
}