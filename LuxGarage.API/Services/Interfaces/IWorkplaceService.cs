using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;

namespace LuxGarage.API.Services.Interfaces
{
    public interface IWorkplaceService
    {
        Task<IEnumerable<WorkplaceResponse>> GetAllAsync();
        Task<WorkplaceResponse?> GetByIdAsync(int id);
        Task<WorkplaceResponse> CreateAsync(ChangeWorkplaceRequest request);
        Task<WorkplaceResponse?> UpdateAsync(ChangeWorkplaceRequest request, int id);
        Task<bool> DeleteAsync(int id);
    }
}
