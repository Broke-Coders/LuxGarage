
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class InsuranceRepository : IInsuranceRepository
{
    private readonly RentalContext _context;

    public InsuranceRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Insurance?> GetByIdAsync(int id) 
        => await _context.Insurances.FindAsync(id);
    
    public async Task AddAsync(Insurance insurance)
    {
        await _context.Insurances.AddAsync(insurance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Insurance insurance, int id)
    {
        Insurance? oldInsurance = await _context.Insurances.FindAsync(id);

        oldInsurance = insurance;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var insurance = await _context.Insurances.FindAsync(id);
        
        if (insurance == null)
        {
            Console.WriteLine("Vehicle insurance with given id not found");
            return;
        }

        _context.Insurances.Remove(insurance);
        await _context.SaveChangesAsync();
    }
}