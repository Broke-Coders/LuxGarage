using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

public class OfferRepository : IOfferRepository
{
    private readonly RentalContext context;

    public OfferRepository(RentalContext context)
    {
        this.context = context;
    }
    
    public async Task<List<Offer>> GetAllAsync()
        => await context.Offers.AsNoTracking().ToListAsync();
    public async Task<Offer?> GetByIdAsync(int id)
        => await context.Offers.FindAsync(id);
    public async Task<Offer?> GetByVehicleIdAsync(int vehicleId)
        => await context.Offers.FirstOrDefaultAsync(o => o.VehicleId == vehicleId);
    public async Task AddAsync(Offer offer)
        => await context.Offers.AddAsync(offer);
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
    public async Task DeleteAsync(int id)
    {
        var offer = await context.Offers.FindAsync(id);
        
        if (offer is null) return;

        context.Offers.Remove(offer);
    }
    public async Task<bool> ExistsAsync(int id)
        => await context.Offers.AnyAsync(o => o.Id == id);
}
