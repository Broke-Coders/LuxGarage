using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing workplace data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting workplace information from the database.
/// </summary>
public class WorkplaceRepository : IWorkplaceRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the WorkplaceRepository class, providing the necessary context for accessing the database
    /// and performing operations on workplace data.
    /// </summary>
    /// <param name="context">The context for accessing the database.</param>
    public WorkplaceRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all workplaces from the database.
    /// </summary>
    /// <returns>A list of all workplaces in the database.</returns>
    public async Task<IEnumerable<Workplace>> GetAllAsync()
        => await _context.Workplaces.ToListAsync();

    /// <summary>
    /// Retrieves a workplace by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the workplace to retrieve.</param>
    /// <returns>The workplace with the specified identifier, or null if not found.</returns>
    public async Task<Workplace?> GetByIdAsync(int id) 
        => await _context.Workplaces.FindAsync(id);

    /// <summary>
    /// Adds a new workplace to the database.
    /// </summary>
    /// <param name="workplace">The workplace to add.</param>
    public async Task AddAsync(Workplace workplace)
    {
        await _context.Workplaces.AddAsync(workplace);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing workplace in the database.
    /// </summary>
    /// <param name="workplace">The updated workplace information.</param>
    /// <param name="id">The unique identifier of the workplace to update.</param>
    public async Task UpdateAsync(Workplace workplace, int id)
    {
        Workplace? oldWorkplace = await _context.Workplaces.FindAsync(id);

        if (oldWorkplace != null) _context.Entry(oldWorkplace).CurrentValues.SetValues(workplace);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a workplace from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the workplace to delete.</param>s
    public async Task DeleteAsync(int id)
    {
        var workplace = await _context.Workplaces.FindAsync(id);
        if (workplace != null)
        {
            _context.Workplaces.Remove(workplace);
            await _context.SaveChangesAsync();
        }
        
    }
}