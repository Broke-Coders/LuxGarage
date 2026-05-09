using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing vehicle image entities in the LuxGarage API,
/// providing methods for retrieving, ordering, updating, and deleting vehicle images.
/// </summary>
public class VehicleImageRepository : IVehicleImageRepository
{
    private readonly RentalContext context;

    /// <summary>
    /// Initializes a new instance of the VehicleImageRepository class with the specified database context.
    /// </summary>
    /// <param name="context">The database context used for accessing vehicle images.</param>
    public VehicleImageRepository(RentalContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Retrieves a vehicle image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle image.</param>
    /// <returns>The vehicle image with the specified identifier, or null if it does not exist.</returns>
    public async Task<VehicleImage?> GetByIdAsync(int id)
        => await context.VehicleImages.FindAsync(id); 

    /// <summary>
    /// Retrieves all images for a specific vehicle.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>A list of vehicle images associated with the specified vehicle.</returns>
    public async Task<List<VehicleImage>> GetByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .AsNoTracking()
                        .ToListAsync();
    
    /// <summary>
    /// Retrieves images for a specific vehicle ordered by sort order.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>An ordered list of vehicle images for the specified vehicle.</returns>
    public async Task<List<VehicleImage>> GetOrderedByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .Where(i => i.VehicleId == vehicleId)
                        .OrderBy(i => i.SortOrder)
                        .AsNoTracking()
                        .ToListAsync();

    /// <summary>
    /// Retrieves the primary vehicle image for a specific vehicle.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>The primary vehicle image, or null if none exists.</returns>
    public async Task<VehicleImage?> GetPrimaryByVehicleIdAsync(int vehicleId)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.SortOrder == 0 && i.VehicleId == vehicleId);

    /// <summary>
    /// Retrieves a vehicle image by its storage key.
    /// </summary>
    /// <param name="key">The storage key associated with the vehicle image.</param>
    /// <returns>The vehicle image with the specified storage key, or null if it does not exist.</returns>
    public async Task<VehicleImage?> GetByStorageKeyAsync(string key)
        => await context.VehicleImages
                        .FirstOrDefaultAsync(i => i.StorageKey == key);

    /// <summary>
    /// Gets the highest sort order value for images belonging to the specified vehicle.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>The maximum sort order for the vehicle's images, or -1 if none exist.</returns>
    public async Task<int> GetMaxSortOrderAsync(int vehicleId)
        => (await context.VehicleImages
                            .Where(i => i.VehicleId == vehicleId)
                            .MaxAsync(i => (int?)i.SortOrder)) ?? -1;

    /// <summary>
    /// Determines whether a primary image already exists for the specified vehicle.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <returns>True if a primary image exists; otherwise, false.</returns>
    public async Task<bool> AnyPrimaryForVehicleAsync(int vehicleId)
        => await context.VehicleImages
                        .AnyAsync(i => i.VehicleId == vehicleId && i.SortOrder == 0);

    /// <summary>
    /// Adds a new vehicle image to the database context.
    /// </summary>
    /// <param name="vehicleImage">The vehicle image to add.</param>
    public async Task AddAsync(VehicleImage vehicleImage)
        => await context.VehicleImages.AddAsync(vehicleImage);

    /// <summary>
    /// Deletes a vehicle image by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle image to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var image = await context.VehicleImages.FindAsync(id);

        if (image is null) return;

        context.VehicleImages.Remove(image);
    }

    /// <summary>
    /// Marks a vehicle image as the primary image by setting its sort order to zero.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle image to mark as primary.</param>
    public async Task SetPrimaryAsync(int id)
        => await UpdateSortOrderAsync(id, 0);
    

    /// <summary>
    /// Updates the sort order of a vehicle image and adjusts the sort orders of other images accordingly.
    /// </summary>
    /// <param name="imageId">The unique identifier of the vehicle image.</param>
    /// <param name="newSortOrder">The new sort order value to apply.</param>
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

    /// <summary>
    /// Reorders the images for a vehicle according to the provided image identifier list.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle.</param>
    /// <param name="orderedImageIds">The ordered list of image identifiers.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the provided identifiers do not match the images for the specified vehicle.
    /// </exception>
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
