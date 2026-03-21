using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class RentalInsuranceRepository : IRentalInsuranceRepository
{
    private readonly RentalContext _context;

    public RentalInsuranceRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<RentalInsurance?> GetByIdAsync(int id) 
        => await _context.RentalInsurances.FindAsync(id);
    
    public async Task AddAsync(RentalInsurance rentalInsurance)
    {
        await _context.RentalInsurances.AddAsync(rentalInsurance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RentalInsurance rentalInsurance, int id)
    {
        RentalInsurance? oldRentalInsurance = await _context.RentalInsurances.FindAsync(id);

        oldRentalInsurance = rentalInsurance;
        await _context.SaveChangesAsync();
    }

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