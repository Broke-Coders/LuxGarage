using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Offers;

[ApiController]
[Route("api/offers")]
public class OffersController : ControllerBase
{
    private readonly OfferService _offerService;

    public OffersController(OfferService offerService)
    {
        _offerService = offerService;
    }

    /// <summary>
    /// Gets list of all offers
    /// </summary>
    [HttpGet]
    [AllowAnonymous] 
    public async Task<ActionResult<List<OfferListItemResponse>>> GetAllOffers([FromQuery] GetOffersRequest request)
    {
        var offers = await _offerService.GetAllOffersAsync(request);
        return Ok(offers);
    }

    /// <summary>
    /// Gets details of selected offer
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<OfferDetailsResponse>> GetOfferById(int id)
    {
        var offer = await _offerService.GetOfferByIdAsync(id);

        if (offer is null)
            return NotFound(new { Message = $"Offer with ID {id} not found." });

        return Ok(offer);
    }

    /// <summary>
    /// Gets car based on its ID
    /// </summary>
    [HttpGet("vehicle/{vehicleId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<OfferDetailsResponse>> GetOfferByVehicleId(int vehicleId)
    {
        var offer = await _offerService.GetByVehicleIdAsync(vehicleId);

        if (offer is null)
            return NotFound(new { Message = $"No offer found for Vehicle ID {vehicleId}." });

        return Ok(offer);
    }

    /// <summary>
    /// Creates a new offer for existing vehicle
    /// Access: Employee and Admin
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Employee, Admin")]
    public async Task<ActionResult> Create([FromBody] CreateOfferRequest request)
    {
        var offerId = await _offerService.CreateOfferAsync(request);
        return CreatedAtAction(nameof(GetOfferById), new { id = offerId }, null);
    }

    /// <summary>
    /// Modifies existing offer
    /// Access: Employee and Admin only
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Employee, Admin")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateOfferRequest request)
    {
        if (!await _offerService.ExistsAsync(id))
            return NotFound(new { Message = $"Offer with ID {id} not found." });

        // Przekazujemy gotowy Request
        await _offerService.UpdateOfferAsync(id, request);
        
        return NoContent();
    }

    /// <summary>
    /// Permanently delets offer from database
    /// Access: Admin only
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _offerService.ExistsAsync(id))
            return NotFound(new { Message = $"Offer with ID {id} not found." });

        await _offerService.DeleteOfferAsync(id);
        return NoContent();
    }
}