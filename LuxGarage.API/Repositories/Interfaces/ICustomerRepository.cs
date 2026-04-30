using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer, int id);
    Task DeleteAsync(int id);
}