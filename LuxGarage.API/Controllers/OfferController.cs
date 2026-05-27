using LuxGarage.API.DTOs.Requests.Offer;
using LuxGarage.API.DTOs.Responses.Offer;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/offers")]
public class OffersController : ControllerBase
{
    private readonly IOfferService _offerService;

    public OffersController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OfferListItemResponse>>> GetAllOffers(
        [FromQuery] GetOffersRequest request)
    {
        var offers = await _offerService.GetAllOffersAsync(request);
        return Ok(offers);
    }

    [HttpGet("{offerId}")]
    public async Task<ActionResult<OfferDetailsResponse>> GetOfferById(int offerId)
    {
        var offer = await _offerService.GetOfferByIdAsync(offerId);

        if (offer is null)
            return NotFound();

        return Ok(offer);
    }
}