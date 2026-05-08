
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing permission data in the LuxGarage API, providing methods for retrieving, adding, updating,
/// and deleting permission information from the database.
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the PermissionRepository class, providing the necessary context for accessing the database
    /// and performing operations on permission data.
    /// </summary>
    /// <param name="context">The database context for accessing permission data.</param>
    public PermissionRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a permission by its id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the permission to retrieve.</param>
    /// <returns>The permission with the specified identifier, or null if not found.</returns>
    public async Task<Permission?> GetByIdAsync(int id) 
        => await _context.Permissions.FindAsync(id);
    
    
    /// <summary>
    /// Adds a new permission to the database.
    /// </summary>
    /// <param name="permission">The permission to add.</param>
    public async Task AddAsync(Permission permission)
    {
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing permission in the database.
    /// </summary>
    /// <param name="permission">The updated permission information.</param>
    /// <param name="id">The unique identifier of the permission to update.</param>
    public async Task UpdateAsync(Permission permission, int id)
    {
        Permission? oldPermission = await _context.Permissions.FindAsync(id);

        oldPermission = permission;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a permission from the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the permission to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        
        if (permission == null)
        {
            Console.WriteLine("permission with given id not found");
            return;
        }

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync();
    }
}