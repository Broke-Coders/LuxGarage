using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class WorkplaceRepository : IWorkplaceRepository
{
    private readonly RentalContext _context;

    public WorkplaceRepository(RentalContext context)
    {
        this._context = context;
    }

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

        oldWorkplace = workplace;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var workplace = await _context.Workplaces.FindAsync(id);
        
        if (workplace == null)
        {
            Console.WriteLine("workplace with given id not found");
            return;
        }

        _context.Workplaces.Remove(workplace);
        await _context.SaveChangesAsync();
    }
}