using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing customer data in the LuxGarage API, providing methods for retrieving, adding, updating, 
/// and deleting customer information from the database. 
/// </summary>
/// <remarks>
/// This class serves as an implementation of the ICustomerRepository interface, 
/// allowing for the interaction with the underlying data context to perform CRUD operations on customer entities. 
/// The repository encapsulates the logic for accessing and manipulating customer data, ensuring that the application can manage customer 
/// information effectively while maintaining a separation of concerns between the data access layer and the business logic layer of the application.
/// </remarks>
/// <summary>
/// Implements the data access layer for customer management using Entity Framework Core.
/// Handles the physical interaction with the database for all customer-related entities.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public CustomerRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Finds a customer by their primary key.
    /// </summary>
    public async Task<Customer?> GetByIdAsync(int id) 
        => await _context.Customers.FindAsync(id);
    
    /// <summary>
    /// Inserts a new customer record into the database.
    /// </summary>
    public async Task AddAsync(Customer borrower)
    {
        await _context.Customers.AddAsync(borrower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing customer record. 
    /// Note: EF Core's Update() handles tracking and modification detection.
    /// </summary>
    public async Task UpdateAsync(Customer borrower, int id)
    {
        _context.Customers.Update(borrower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes a customer from the database.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var borrower = await _context.Customers.FindAsync(id);
        
        if (borrower == null)
        {
            return;
        }

        _context.Customers.Remove(borrower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all customers, optimizing performance with AsNoTracking.
    /// </summary>
    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Performs an asynchronous lookup for a customer by their email.
    /// </summary>
    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }
}