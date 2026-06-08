using AutoMapper;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Offers;

public class OfferService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public OfferService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a list of all offers with predefined sorting applied.
    /// </summary>
    public async Task<List<OfferListItemResponse>> GetAllOffersAsync(GetOffersRequest request)
    {
        var query = _context.Offers
            .Include(o => o.Vehicle)
            .AsNoTracking()
            .AsQueryable();

        query = ApplySorting(query, request);

        var offers = await query.ToListAsync();
        var listDtos = _mapper.Map<List<OfferListItemResponse>>(offers);

        foreach (var dto in listDtos)
        {
            dto.PrimaryImageUrl = $"/api/vehicle-images/by-offer/{dto.Id}/primary/file";
        }

        return listDtos;
    }

    /// <summary>
    /// Retrieves detailed information for a specific offer by its ID.
    /// </summary>
    public async Task<OfferDetailsResponse?> GetOfferByIdAsync(int offerId)
    {
        var offer = await _context.Offers
            .Include(o => o.Vehicle)
            .Include(o => o.Vehicle.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == offerId);

        return offer is null ? null : _mapper.Map<OfferDetailsResponse>(offer);
    }

    /// <summary>
    /// Retrieves an offer associated with a specific vehicle ID.
    /// </summary>
    public async Task<OfferDetailsResponse?> GetByVehicleIdAsync(int vehicleId)
    {
        var offer = await _context.Offers
            .Include(o => o.Vehicle)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.VehicleId == vehicleId);

        return offer is null ? null : _mapper.Map<OfferDetailsResponse>(offer);
    }

    /// <summary>
    /// Creates a new offer and saves it to the database.
    /// </summary>
    public async Task<int> CreateOfferAsync(Offer offer)
    {
        await _context.Offers.AddAsync(offer);
        await _context.SaveChangesAsync();
        
        return offer.Id;
    }

    /// <summary>
    /// Updates an existing offer's details.
    /// </summary>
    public async Task UpdateOfferAsync(int id, Offer updatedOfferData)
    {
        var existingOffer = await _context.Offers.FindAsync(id);

        if (existingOffer is null) return;

        existingOffer.Title = updatedOfferData.Title;
        existingOffer.Description = updatedOfferData.Description;
        existingOffer.PricePerDay = updatedOfferData.PricePerDay;
        existingOffer.IsActive = updatedOfferData.IsActive;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an offer from the database by its ID.
    /// </summary>
    public async Task DeleteOfferAsync(int id)
    {
        await _context.Offers.Where(o => o.Id == id).ExecuteDeleteAsync();
    }

    /// <summary>
    /// Checks whether an offer exists in the database.
    /// </summary>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Offers.AnyAsync(o => o.Id == id);
    }

    private static IQueryable<Offer> ApplySorting(IQueryable<Offer> query, GetOffersRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            ("price", false) => query.OrderBy(o => o.PricePerDay),
            ("price", true) => query.OrderByDescending(o => o.PricePerDay),
            ("brand", false) => query.OrderBy(o => o.Vehicle.Brand),
            ("brand", true) => query.OrderByDescending(o => o.Vehicle.Brand),
            ("mileage", false) => query.OrderBy(o => o.Vehicle.Mileage),
            ("mileage", true) => query.OrderByDescending(o => o.Vehicle.Mileage),
            _ => request.Descending
                ? query.OrderByDescending(o => o.Id)
                : query.OrderBy(o => o.Id)
        };
    }
}