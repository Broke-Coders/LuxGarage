using LuxGarage.API.DTOs.VehicleImage.Requests;
using LuxGarage.API.DTOs.VehicleImage.Responses;

namespace LuxGarage.API.Services.Interfaces;

/// <summary>
/// Defines the interface for the vehicle image service in the LuxGarage API, providing methods for managing vehicle images,
/// including retrieval, uploading, sorting, and deletion of images, while ensuring proper handling of 
/// vehicle image-related operations and returning appropriate responses for each action.
/// </summary>
public interface IVehicleImageService
{
    Task<List<VehicleImageResponse>> GetByVehicleIdAsync(int vehicleId);
    Task<List<VehicleImageResponse>> GetOrderedByVehicleIdAsync(int vehicleId);
    Task<List<VehicleImageResponse>> GetPrimaryByVehicleIdAsync(int vehicleId);

    Task<VehicleImageResponse> UploadAsync(CreateVehicleImageRequest request);
    Task<List<VehicleImageResponse>> UploadManyAsync(CreateManyImagesRequest request);
    
    Task SetPrimaryAsync(SetPrimaryImageRequest request);
    Task UpdateSortOrderAsync(UpdateSortOrderRequest request);
    Task ReorderAsync(ReorderImagesRequest request);

    Task DeleteAsync(int imageId);
}
