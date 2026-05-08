using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

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
public class CustomerRepository : ICustomerRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the CustomerRepository class, providing the necessary context for accessing the database 
    /// and performing operations on customer data.
    /// </summary>
    /// <param name="context">context for accessing the database</param>
    public CustomerRepository(RentalContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier from the database, 
    /// allowing for the retrieval of specific customer information based on the provided ID.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to retrieve.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    public async Task<Customer?> GetByIdAsync(int id) 
        => await _context.Customers.FindAsync(id);
    
    /// <summary>
    /// Adds a new customer to the database, allowing for the creation of new customer records in the system.
    /// </summary>
    /// <param name="borrower">The customer to add.</param>
    public async Task AddAsync(Customer borrower)
    {
        await _context.Customers.AddAsync(borrower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing customer's information in the database, 
    /// allowing for the modification of customer records based on the provided customer data and ID.
    /// </summary>
    /// <param name="borrower">The updated customer information.</param>
    /// <param name="id">The unique identifier of the customer to update.</param>
    public async Task UpdateAsync(Customer borrower, int id)
    {
        Customer? oldBorrower = await _context.Customers.FindAsync(id);

        oldBorrower = borrower;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a customer from the database based on their unique identifier, 
    /// allowing for the removal of customer records from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
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