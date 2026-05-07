using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

public class WorkplaceRepository : IWorkplaceRepository
{
    private readonly RentalContext _context;

    public WorkplaceRepository(RentalContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workplace>> GetAllAsync()
        => await _context.Workplaces.ToListAsync();

    public async Task<Workplace?> GetByIdAsync(int id) 
        => await _context.Workplaces.FindAsync(id);
    public async Task AddAsync(Workplace workplace)
    {
        await _context.Workplaces.AddAsync(workplace);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Workplace workplace, int id)
    {
        Workplace? oldWorkplace = await _context.Workplaces.FindAsync(id);

        if (oldWorkplace != null) _context.Entry(oldWorkplace).CurrentValues.SetValues(workplace);
        await _context.SaveChangesAsync();
    }

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