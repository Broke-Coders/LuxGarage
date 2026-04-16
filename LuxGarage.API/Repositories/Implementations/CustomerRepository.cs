using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class CustomerRepository : ICustomerRepository
{
    private readonly RentalContext _context;

    public CustomerRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id) 
        => await _context.Customers.FindAsync(id);
    
    public async Task AddAsync(Customer borrower)
    {
        await _context.Customers.AddAsync(borrower);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer borrower, int id)
    {
        Customer? oldBorrower = await _context.Customers.FindAsync(id);

        oldBorrower = borrower;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var borrower = await _context.Customers.FindAsync(id);
        
        if (borrower == null)
        {
            Console.WriteLine("borrower with given id not found");
            return;
        }

        _context.Customers.Remove(borrower);
        await _context.SaveChangesAsync();
    }
}