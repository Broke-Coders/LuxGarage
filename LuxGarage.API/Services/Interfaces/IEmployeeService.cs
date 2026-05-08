using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Services.Interfaces
{
    /// <summary>
    /// Defines the interface for the employee service in the LuxGarage API, providing methods for managing employee data,
    /// including retrieval, updating, deletion, and password changes, while ensuring proper handling of 
    /// employee-related operations and returning appropriate responses for each action.
    /// </summary>
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse?> GetByIdAsync(int id);

        Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangePasswordAsync(int id, ChangePasswordRequest request);
    }
}
