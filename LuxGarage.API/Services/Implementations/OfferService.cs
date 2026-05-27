using AutoMapper;
using LuxGarage.API.Models;
using LuxGarage.API.DTOs.Requests.Offer;
using LuxGarage.API.DTOs.Responses.Offer;
using LuxGarage.API.Services.Interfaces;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public OfferService(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<List<OfferListItemResponse>> GetAllOffersAsync(GetOffersRequest request)
    {
        var offers = await _offerRepository.GetAllWithVehicleAsync();

        offers = ApplySorting(offers, request);

        var listDtos = _mapper.Map<List<OfferListItemResponse>>(offers);

        foreach (var dto in listDtos)
        {
            dto.PrimaryImageUrl = $"/api/vehicle-images/by-offer/{dto.Id}/primary/file";
        }

        return listDtos;
    }

    public async Task<OfferDetailsResponse?> GetOfferByIdAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdWithVehicleAndImagesAsync(offerId);

        if (offer is null)
            return null;

        var dto = _mapper.Map<OfferDetailsResponse>(offer);

        return dto;
    }

    private static List<Offer> ApplySorting(List<Offer> offers, GetOffersRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            //("priceDay", false) => offers.OrderBy(o => o.PricePerDay).ToList(),
            //("priceDay", true) => offers.OrderByDescending(o => o.PricePerDay).ToList(),

            //("priceWeek", false) => offers.OrderBy(o => o.PricePerWeek).ToList(),
            //("priceWeek", true) => offers.OrderByDescending(o => o.PricePerWeek).ToList(),

            ("brand", false) => offers.OrderBy(o => o.Vehicle.VehicleBrand.Name).ToList(),
            ("brand", true) => offers.OrderByDescending(o => o.Vehicle.VehicleBrand.Name).ToList(),

            ("mileage", false) => offers.OrderBy(o => o.Vehicle.Mileage).ToList(),
            ("mileage", true) => offers.OrderByDescending(o => o.Vehicle.Mileage).ToList(),

            _ => request.Descending
                ? offers.OrderByDescending(o => o.Id).ToList()
                : offers.OrderBy(o => o.Id).ToList()
        };
    }
}