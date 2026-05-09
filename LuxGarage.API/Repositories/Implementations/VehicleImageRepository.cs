using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

public class VehicleImageRepository : IVehicleImageRepository
{

    private readonly RentalContext context;

    VehicleImageRepository(RentalContext context)
    {
        this.context = context;
    }

    public async Task<VehicleImage?> GetByIdAsync(int id)
        => await context.VehicleImages.FindAsync(id); 

    public async Task<List<VehicleImage>> GetByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .ToListAsync();
    
    public async Task<List<VehicleImage>> GetOrderedByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .OrderBy(i => i.SortOrder)
                        .ToListAsync();

    public async Task<VehicleImage?> GetPrimaryByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.SortOrder == 0 && i.VehicleId == vehicleId); 
    public async Task<VehicleImage?> GetByStorageKeyAsync(string key)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.StorageKey == key);

    public async Task<int> GetMaxSortOrderAsync(int vehicleId)
        => await context.VehicleImages
                            .Where(i => i.VehicleId == vehicleId)
                            .MaxAsync(i => i.SortOrder);

    public async Task<bool> AnyPrimaryForVehicleAsync(int vehicleId)
        => await context.VehicleImages
                        .AnyAsync(i => i.VehicleId == vehicleId && i.SortOrder == 0);

    public async Task AddAsync(VehicleImage vehicleImage)
    {
        await context.VehicleImages.AddAsync(vehicleImage);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var image = await context.VehicleImages.FindAsync(id);

        if (image is null) return;

        context.VehicleImages.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task SetPrimaryAsync(int id)
    {
       var image = await context.VehicleImages.FindAsync(id); 

       if (image is null) return;
       
       image.SortOrder = 0;
    }

    public async Task UpdateSortOrderAsync(int imageId, int sortOrder)
    {

    }

    public async Task ReorderAsync(int vehicleId, List<int> orderedImageIds)
    {

    }
}
