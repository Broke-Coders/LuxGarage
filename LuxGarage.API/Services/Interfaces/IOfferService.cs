using LuxGarage.API.DTOs.Requests.Offer;
using LuxGarage.API.DTOs.Responses.Offer;

namespace LuxGarage.API.Services.Interfaces;

public interface IOfferService
{
    Task<List<OfferListItemResponse>> GetAllOffersAsync(GetOffersRequest request);
    Task<OfferDetailsResponse?> GetOfferByIdAsync(int offerId);
}