using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee, int id);
    Task DeleteAsync(int id);
}