using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages customer data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting customer information from the database.
/// </summary>
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer, int id);
    Task DeleteAsync(int id);
}