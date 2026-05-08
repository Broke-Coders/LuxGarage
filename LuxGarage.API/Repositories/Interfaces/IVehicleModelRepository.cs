using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that manages vehicle model data in the LuxGarage API,
    /// providing methods for retrieving, adding, updating, and deleting vehicle model information from the database.
    /// </summary>
    public interface IVehicleModelRepository
    {

        Task<VehicleModel?> GetByIdAsync(int id);

        Task AddAsync(VehicleModel model);

        Task UpdateAsync(VehicleModel model, int id);
        Task DeleteAsync(int id);


    }
}
