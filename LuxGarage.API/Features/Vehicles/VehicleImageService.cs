using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Vehicles;

public class VehicleImageService
{
    private readonly RentalContext _context;
    private readonly string _uploadFolder;

    public VehicleImageService(RentalContext context, IWebHostEnvironment env)
    {
        _context = context;
        _uploadFolder = Path.Combine(env.ContentRootPath, "..", "LuxGarage.Front", "wwwroot", "images", "cars");
    }

    public async Task<List<VehicleImageResponse>> GetByVehicleIdAsync(int vehicleId)
    {
        var images = await _context.VehicleImages
            .Where(i => i.VehicleId == vehicleId)
            .OrderBy(i => i.SortOrder)
            .AsNoTracking()
            .ToListAsync();

        return images.Select(MapToResponse).ToList();
    }

    public async Task<List<VehicleImageResponse>> UploadImagesAsync(UploadVehicleImagesRequest request)
    {
        var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == request.VehicleId);
        if (!vehicleExists)
            throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} does not exist.");

        var vehicleFolder = Path.Combine(_uploadFolder, request.VehicleId.ToString());
        if (!Directory.Exists(vehicleFolder))
            Directory.CreateDirectory(vehicleFolder);

        var currentMaxSortOrder = await _context.VehicleImages
            .Where(i => i.VehicleId == request.VehicleId)
            .MaxAsync(i => (int?)i.SortOrder) ?? -1;

        bool isFirstUploadForVehicle = !await _context.VehicleImages
            .AnyAsync(i => i.VehicleId == request.VehicleId);

        var newEntities = new List<VehicleImage>();

        for (int i = 0; i < request.Images.Count; i++)
        {
            var file = request.Images[i];
            if (file.Length == 0) continue;

            var extension = Path.GetExtension(file.FileName);
            var storageKey = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(vehicleFolder, storageKey);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            currentMaxSortOrder++;

            bool isPrimary = false;
            if (request.PrimaryImageIndex.HasValue && request.PrimaryImageIndex.Value == i)
            {
                isPrimary = true;
            }
            else if (isFirstUploadForVehicle && i == 0 && !request.PrimaryImageIndex.HasValue)
            {
                isPrimary = true;
            }

            var entity = new VehicleImage
            {
                VehicleId = request.VehicleId,
                StorageKey = storageKey,
                OriginalFileName = Path.GetFileName(file.FileName),
                ContentType = file.ContentType,
                FileSize = file.Length,
                SortOrder = currentMaxSortOrder,
                IsPrimary = isPrimary
            };
            
            newEntities.Add(entity);
        }

        await _context.VehicleImages.AddRangeAsync(newEntities);
        await _context.SaveChangesAsync();

        return newEntities.Select(MapToResponse).ToList();
    }

    public async Task SetPrimaryAsync(int vehicleId, int imageId)
    {
        await _context.VehicleImages
            .Where(i => i.VehicleId == vehicleId && i.IsPrimary)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsPrimary, false));

        var updatedRows = await _context.VehicleImages
            .Where(i => i.Id == imageId && i.VehicleId == vehicleId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsPrimary, true));

        if (updatedRows == 0)
            throw new KeyNotFoundException("Image not found or does not belong to this vehicle.");
    }

    public async Task ReorderAsync(ReorderImagesRequest request)
    {
        var existingImages = await _context.VehicleImages
            .Where(i => i.VehicleId == request.VehicleId)
            .ToListAsync();

        if (existingImages.Count != request.OrderedImageIds.Count)
            throw new InvalidOperationException("Provided list length does not match vehicle images count.");

        var dict = existingImages.ToDictionary(x => x.Id);

        for (int i = 0; i < request.OrderedImageIds.Count; i++)
        {
            var id = request.OrderedImageIds[i];
            if (dict.TryGetValue(id, out var image))
            {
                image.SortOrder = i;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int imageId)
    {
        var image = await _context.VehicleImages.FindAsync(imageId);
        if (image is null) return;

        _context.VehicleImages.Remove(image);
        await _context.SaveChangesAsync();

        var filePath = Path.Combine(_uploadFolder, image.VehicleId.ToString(), image.StorageKey);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private VehicleImageResponse MapToResponse(VehicleImage entity)
    {
        return new VehicleImageResponse
        {
            Id = entity.Id,
            VehicleId = entity.VehicleId,
            StorageKey = entity.StorageKey,
            OriginalFileName = entity.OriginalFileName,
            FileSize = entity.FileSize,
            SortOrder = entity.SortOrder,
            IsPrimary = entity.IsPrimary,
            // Przykładowy URL 
            Url = $"/api/vehicleimages/{entity.Id}/file"
        };
    }
}