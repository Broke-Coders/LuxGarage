using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;

namespace LuxGarage.API.Services.Interfaces
{
    /// <summary>
    /// Defines the interface for the workplace service in the LuxGarage API, providing methods for managing workplace data,
    /// including retrieval, creation, updating, and deletion of workplaces, while ensuring proper validation
    /// for workplace data and returning appropriate responses for each action.
    /// </summary>
    public interface IWorkplaceService
    {
        Task<IEnumerable<WorkplaceResponse>> GetAllAsync();
        Task<WorkplaceResponse?> GetByIdAsync(int id);
        Task<WorkplaceResponse> CreateAsync(ChangeWorkplaceRequest request);
        Task<WorkplaceResponse?> UpdateAsync(ChangeWorkplaceRequest request, int id);
        Task<bool> DeleteAsync(int id);
    }
}
