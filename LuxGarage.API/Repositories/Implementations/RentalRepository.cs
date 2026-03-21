

using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class RentalRepository : IRentalRepository
{
    private readonly RentalContext _context;

    public RentalRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Rental?> GetByIdAsync(int id) 
        => await _context.Rentals.FindAsync(id);
    
    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rental rental, int id)
    {
        Rental? oldRental = await _context.Rentals.FindAsync(id);

        oldRental = rental;
        await _context.SaveChangesAsync();
    }

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