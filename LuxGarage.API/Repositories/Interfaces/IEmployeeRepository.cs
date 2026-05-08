using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByLoginAsync(string login);
    Task AddAsync(Employee employee);
    Task UpdateAsync(int id, Employee employee);
    Task DeleteAsync(int id);
}