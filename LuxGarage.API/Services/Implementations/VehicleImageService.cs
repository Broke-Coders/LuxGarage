using LuxGarage.API.DTOs.VehicleImage.Responses;
using LuxGarage.API.DTOs.VehicleImage.Requests;
using LuxGarage.API.Repositories.Interfaces;
using AutoMapper;
using LuxGarage.API.Models;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations;

public class VehicleImageService : IVehicleImageService
{
    private readonly IVehicleImageRepository repo;
    private readonly IMapper mapper;

    public VehicleImageService(IVehicleImageRepository repository, IMapper mapper)
    {
        this.repo = repository;
        this.mapper = mapper;
    }

    public async Task<List<VehicleImageResponse>> GetByVehicleIdAsync(int vehicleId)
    {
        var images = await repo.GetByVehicleIdAsync(vehicleId);
        
        if (images is null)
        {
            throw new Exception($"There's no images with given vehicle ID {vehicleId}");
        }

        return mapper.Map<List<VehicleImageResponse>>(images);
    }
    
    public async Task<List<VehicleImageResponse>> GetOrderedByVehicleIdAsync(int vehicleId)
    {
        var images = await repo.GetOrderedByVehicleIdAsync(vehicleId);

        if (images is null)
        {
            throw new Exception($"There's no images with given vehicle ID {vehicleId}");
        }

        return mapper.Map<List<VehicleImageResponse>>(images);
    }
    
    public async Task<List<VehicleImageResponse>> GetPrimaryByVehicleIdAsync(int vehicleId)
    {
        var images = await repo.GetPrimaryByVehicleIdAsync(vehicleId);

        if (images is null)
        {
            throw new Exception($"There's no images with given vehicle ID {vehicleId}");
        }

        return mapper.Map<List<VehicleImageResponse>>(images);
    }

    public async Task<VehicleImageResponse> UploadAsync(CreateVehicleImageRequest request)
    {
        VehicleImage image = new();

        image.VehicleId = request.VehicleId;
        image.StorageKey = $"{Guid.NewGuid()}_{request.Image.FileName}";
        image.ContentType = request.Image.ContentType;
        image.FileSize = request.Image.Length;
        image.SortOrder = await repo.GetMaxSortOrderAsync(request.VehicleId) + 1;

        await repo.AddAsync(image);
        
        return mapper.Map<VehicleImageResponse>(image);

    }
    public async Task<List<VehicleImageResponse>> UploadManyAsync(CreateManyImagesRequest request)
    {
        List<VehicleImageResponse> newImages = new();

        foreach (var image in request.Images)
        {
            CreateVehicleImageRequest singleReq = new()
            {
                Image = image,
                VehicleId = request.VehicleId
            };
            newImages.Append(await UploadAsync(singleReq));
        } 

        return newImages;
    }
    
    public async Task SetPrimaryAsync(SetPrimaryImageRequest request)
    {
        var image = await repo.GetByIdAsync(request.ImageId);

        if (image is null)
        {
            throw new Exception($"There is no image with given ID: {request.ImageId}");
        }

        await repo.SetPrimaryAsync(request.ImageId);
    }

    public async Task UpdateSortOrderAsync(UpdateSortOrderRequest request)
    {
        var image = await repo.GetByIdAsync(request.ImageId);

        if (image is null)
        {
            throw new Exception($"There is no image with given ID: {request.ImageId}");
        }

        await repo.SetPrimaryAsync(request.ImageId);
    }
    public async Task ReorderAsync(ReorderImagesRequest request)
    {
        var images = await repo.GetByVehicleIdAsync(request.VehicleId);

        if (images is null)
        {
            throw new Exception($"There are no images with given vehicle ID: {request.VehicleId}");
        }

        await repo.ReorderAsync(request.VehicleId, request.OrderedIds);
    }


    public async Task DeleteAsync(int imageId)
    {
        var image = await repo.GetByIdAsync(imageId);

        if (image is null)
        {
            throw new Exception($"There is no image with given ID: {imageId}");
        }

        await repo.DeleteAsync(imageId);
    }
}