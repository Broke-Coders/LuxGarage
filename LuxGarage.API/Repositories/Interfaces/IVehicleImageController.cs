using LuxGarage.API.Models;

namespace LuxGarage.API.Repositories.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle image data in the LuxGarage API, 
/// providing methods for retrieving, adding, updating, and deleting vehicle image information from the database.
/// </summary>
public interface IVehicleImageRepository
{
    Task<VehicleImage?> GetByIdAsync(int id);

    Task<List<VehicleImage>> GetByVehicleIdAsync(int vehicleId);
    Task<List<VehicleImage>> GetOrderedByVehicleIdAsync(int vehicleId);

    Task<VehicleImage?> GetPrimaryByVehicleIdAsync(int vehicleId);
    Task<VehicleImage?> GetByStorageKeyAsync(string key);

    Task<int> GetMaxSortOrderAsync(int vehicleId);
    Task<bool> AnyPrimaryForVehicleAsync(int vehicleId); 

    Task AddAsync(VehicleImage vehicleImage);
    Task DeleteAsync(int id);

    Task SetPrimaryAsync(int id);
    Task UpdateSortOrderAsync(int imageId, int sortOrder);
    Task ReorderAsync(int vehicleId, List<int> orderedImageIds);
}