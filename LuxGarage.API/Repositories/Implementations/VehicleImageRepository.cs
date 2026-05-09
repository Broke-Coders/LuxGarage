using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleImageRepository : IVehicleImageRepository
{
    private readonly RentalContext context;

    public VehicleImageRepository(RentalContext context)
    {
        this.context = context;
    }

    public async Task<VehicleImage?> GetByIdAsync(int id)
        => await context.VehicleImages.FindAsync(id); 

    public async Task<List<VehicleImage>> GetByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .AsNoTracking()
                        .ToListAsync();
    
    public async Task<List<VehicleImage>> GetOrderedByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .OrderBy(i => i.SortOrder)
                        .AsNoTracking()
                        .ToListAsync();

    public async Task<VehicleImage?> GetPrimaryByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.SortOrder == 0 && i.VehicleId == vehicleId);
    public async Task<VehicleImage?> GetByStorageKeyAsync(string key)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.StorageKey == key);

    public async Task<int> GetMaxSortOrderAsync(int vehicleId)
        => (await context.VehicleImages
                            .Where(i => i.VehicleId == vehicleId)
                            .MaxAsync(i => (int?)i.SortOrder)) ?? -1;

    public async Task<bool> AnyPrimaryForVehicleAsync(int vehicleId)
        => await context.VehicleImages
                        .AnyAsync(i => i.VehicleId == vehicleId && i.SortOrder == 0);

    public async Task AddAsync(VehicleImage vehicleImage)
    {
        await context.VehicleImages.AddAsync(vehicleImage);
    }

    public async Task DeleteAsync(int id)
    {
        var image = await context.VehicleImages.FindAsync(id);

        if (image is null) return;

        context.VehicleImages.Remove(image);
    }

    public async Task SetPrimaryAsync(int id)
        => await UpdateSortOrderAsync(id, 0);
    

    public async Task UpdateSortOrderAsync(int imageId, int newSortOrder)
    {
        var image = await context.VehicleImages.FindAsync(imageId);

        if (image is null) return;

        var maxSortOrder = await context.VehicleImages
            .Where(i => i.VehicleId == image.VehicleId)
            .MaxAsync(i => (int?)i.SortOrder) ?? 0;

        if (newSortOrder < 0)
            newSortOrder = 0;

        if (newSortOrder > maxSortOrder)
            newSortOrder = maxSortOrder;

        var oldSortOrder = image.SortOrder;

        if (oldSortOrder == newSortOrder)
            return;

        if (newSortOrder < oldSortOrder)
        {
            await context.VehicleImages
                .Where(i => i.VehicleId == image.VehicleId
                        && i.Id != image.Id
                        && i.SortOrder >= newSortOrder
                        && i.SortOrder < oldSortOrder)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(i => i.SortOrder, i => i.SortOrder + 1));
        }
        else
        {
            await context.VehicleImages
                .Where(i => i.VehicleId == image.VehicleId
                        && i.Id != image.Id
                        && i.SortOrder > oldSortOrder
                        && i.SortOrder <= newSortOrder)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(i => i.SortOrder, i => i.SortOrder - 1));
        }

        image.SortOrder = newSortOrder;

        await context.SaveChangesAsync();
    }

    public async Task ReorderAsync(int vehicleId, List<int> orderedImageIds)
    {
        var images = await context.VehicleImages
                                .Where(i => i.VehicleId == vehicleId)
                                .ToListAsync();
        
        if (images.Count != orderedImageIds.Count) return;

        var existingIds = images.Select(x => x.Id).OrderBy(x => x).ToList();
        var providedIds = orderedImageIds.OrderBy(x => x).ToList();

        if (!existingIds.SequenceEqual(providedIds))
            throw new InvalidOperationException($"Provided id list doesn't match images id with vehicle ID = {vehicleId}");

        var imageMap = images.ToDictionary(x => x.Id);

        for (int i = 0; i < orderedImageIds.Count; i++)
        {
            imageMap[orderedImageIds[i]].SortOrder = i;
        }

        await context.SaveChangesAsync();
    }
}
