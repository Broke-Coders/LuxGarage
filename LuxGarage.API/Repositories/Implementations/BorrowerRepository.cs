using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class BorrowerRepository : IBorrowerRepository
{
    private readonly RentalContext _context;

    public BorrowerRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Borrower?> GetByIdAsync(int id) 
        => await _context.Borrowers.FindAsync(id);
    
    public async Task AddAsync(Borrower borrower)
    {
        await _context.Borrowers.AddAsync(borrower);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Borrower borrower, int id)
    {
        Borrower? oldBorrower = await _context.Borrowers.FindAsync(id);

        oldBorrower = borrower;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var borrower = await _context.Borrowers.FindAsync(id);
        
        if (borrower == null)
        {
            Console.WriteLine("borrower with given id not found");
            return;
        }

        _context.Borrowers.Remove(borrower);
        await _context.SaveChangesAsync();
    }
}