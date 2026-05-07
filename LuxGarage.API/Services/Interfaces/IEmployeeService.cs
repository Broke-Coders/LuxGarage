using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse?> GetByIdAsync(int id);

        Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request);

        Task<bool> DeleteAsync(int id);
    }
}
