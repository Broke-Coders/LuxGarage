using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing offer data in the LuxGarage API,
/// providing methods for retrieving, adding, updating, and deleting offer records.
/// </summary>
public class OfferRepository : IOfferRepository
{
    private readonly RentalContext context;

    /// <summary>
    /// Initializes a new instance of the OfferRepository class with the specified database context.
    /// </summary>
    /// <param name="context">The database context used for accessing offers.</param>
    public OfferRepository(RentalContext context)
    {
        this.context = context;
    }
    
    /// <summary>
    /// Retrieves all offers from the database.
    /// </summary>
    /// <returns>A list of all offers.</returns>
    public async Task<List<Offer>> GetAllAsync()
        => await context.Offers.AsNoTracking().ToListAsync();

    /// <summary>
    /// Retrieves an offer by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the offer.</param>
    /// <returns>The offer with the specified identifier, or null if it does not exist.</returns>
    public async Task<Offer?> GetByIdAsync(int id)
        => await context.Offers.FindAsync(id);

    /// <summary>
    /// Retrieves an offer for a given vehicle.
    /// </summary>
    /// <param name="vehicleId">The unique identifier of the vehicle associated with the offer.</param>
    /// <returns>The offer for the specified vehicle, or null if none exists.</returns>
    public async Task<Offer?> GetByVehicleIdAsync(int vehicleId)
        => await context.Offers.FirstOrDefaultAsync(o => o.VehicleId == vehicleId);

    /// <summary>
    /// Adds a new offer entity to the database context.
    /// </summary>
    /// <param name="offer">The offer to add.</param>
    public async Task AddAsync(Offer offer)
        => await context.Offers.AddAsync(offer);

    /// <summary>
    /// Updates an existing offer in the database.
    /// </summary>
    /// <param name="offer">The updated offer information.</param>
    public async Task UpdateAsync(Offer offer)
    {
        var existing = await context.Offers.FindAsync(offer.Id);

        if (existing is null) return;

        existing.VehicleId = offer.VehicleId;
        existing.VehicleStatusId = offer.VehicleStatusId;
        existing.Description = offer.Description;
        existing.PublicationDate = offer.PublicationDate;
        existing.Prices = offer.Prices;

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an offer by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the offer to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var offer = await context.Offers.FindAsync(id);
        
        if (offer is null) return;

        context.Offers.Remove(offer);
    }

    /// <summary>
    /// Checks whether an offer exists in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the offer.</param>
    /// <returns>True if the offer exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(int id)
        => await context.Offers.AnyAsync(o => o.Id == id);
}
