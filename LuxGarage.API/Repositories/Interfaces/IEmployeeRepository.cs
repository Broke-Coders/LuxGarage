using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages employee data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting employee information from the database.
/// </summary>
public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByLoginAsync(string login);
    Task AddAsync(Employee employee);
    Task UpdateAsync(int id, Employee employee);
    Task DeleteAsync(int id);
}