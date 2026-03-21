
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class PermissionRepository : IPermissionRepository
{
    private readonly RentalContext _context;

    public PermissionRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Permission?> GetByIdAsync(int id) 
        => await _context.Permissions.FindAsync(id);
    
    public async Task AddAsync(Permission permission)
    {
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permission permission, int id)
    {
        Permission? oldPermission = await _context.Permissions.FindAsync(id);

        oldPermission = permission;
        await _context.SaveChangesAsync();
    }

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