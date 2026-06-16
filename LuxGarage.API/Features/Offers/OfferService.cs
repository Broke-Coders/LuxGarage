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
            .Include(o => o.Prices)
            .AsNoTracking()
            .AsQueryable();

        query = ApplySorting(query, request);

        var offers = await query.ToListAsync();
        var listDtos = _mapper.Map<List<OfferListItemResponse>>(offers);

        foreach (var (dto, offer) in listDtos.Zip(offers))
        {
            dto.Price = offer.Prices
                    .OrderByDescending(p => p.ValidFrom)
                    .Select(p => p.PricePerDay)
                    .FirstOrDefault();
            dto.PrimaryImageUrl = $"/api/VehicleImages/vehicle/{dto.VehicleId}/primary";
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
    public async Task<int> CreateOfferAsync(CreateOfferRequest request)
    {
        var offer = new Offer
        {
            VehicleId = request.VehicleId,
            Title = request.Title,
            Description = request.Description,
            PublicationDate = DateTime.UtcNow,
            IsActive = true,
            Prices = new List<OfferPrice>
            {
                new OfferPrice 
                { 
                    PricePerDay = request.InitialPricePerDay,
                    ValidFrom = DateTime.UtcNow
                }
            }
        };
        await _context.Offers.AddAsync(offer);
        await _context.SaveChangesAsync();
        
        return offer.Id;
    }

    /// <summary>
    /// Updates an existing offer's details.
    /// </summary>
  public async Task UpdateOfferAsync(int id, UpdateOfferRequest request)
    {
        var existingOffer = await _context.Offers
            .Include(o => o.Prices)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (existingOffer is null) return;

        existingOffer.Title = request.Title;
        existingOffer.Description = request.Description;
        existingOffer.IsActive = request.IsActive;

        if (request.NewPricePerDay.HasValue)
        {
            var activePrice = existingOffer.Prices.FirstOrDefault(p => p.ValidTo == null);
            
            if (activePrice == null || activePrice.PricePerDay != request.NewPricePerDay.Value)
            {
                var now = DateTime.UtcNow;
                
                if (activePrice != null)
                {
                    activePrice.ValidTo = now;
                }

                existingOffer.Prices.Add(new OfferPrice
                {
                    PricePerDay = request.NewPricePerDay.Value,
                    ValidFrom = now
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an offer from the database by its ID.
    /// </summary>
    public async Task DeleteOfferAsync(int id)
    {
        var offer = await _context.Offers.FindAsync(id);
        if (offer != null)
        {
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
        }
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