using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the data access contract for managing customer information in the database.
/// Provides a foundation for CRUD operations and specialized queries like email lookups.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Retrieves a single customer record by its unique database identifier.
    /// </summary>
    /// <param name="id">The customer's ID.</param>
    /// <returns>The <see cref="Customer"/> if found; otherwise, null.</returns>
    Task<Customer?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a complete list of all customers.
    /// </summary>
    /// <returns>A list of all customer records.</returns>
    Task<List<Customer>> GetAllAsync();

    /// <summary>
    /// Finds a customer based on their unique email address. Useful for validation and login scenarios.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The matching <see cref="Customer"/> if found; otherwise, null.</returns>
    Task<Customer?> GetByEmailAsync(string email);

    /// <summary>
    /// Persists a new customer record to the database.
    /// </summary>
    /// <param name="customer">The customer entity to add.</param>
    Task AddAsync(Customer customer);

    /// <summary>
    /// Synchronizes changes made to a customer entity back to the database.
    /// </summary>
    /// <param name="customer">The entity with updated values.</param>
    /// <param name="id">The ID of the customer record to update.</param>
    Task UpdateAsync(Customer customer, int id);

    /// <summary>
    /// Permanently removes a customer record from the database.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    Task DeleteAsync(int id);
}