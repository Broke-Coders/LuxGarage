using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Interfaces
{
    public interface IVehicleModelRepository
    {

        Task<VehicleModel?> GetByIdAsync(int id);

        Task AddAsync(VehicleModel model);

        Task UpdateAsync(VehicleModel model, int id);
        Task DeleteAsync(int id);


    }
}
