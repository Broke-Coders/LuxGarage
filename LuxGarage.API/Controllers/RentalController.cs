using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Features.Rentals;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly RentalService _rentalService;

    public RentalController(RentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet]
    [Authorize(Roles = "Employee, Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RentalResponse>>>> GetAll()
    {
        try
        {
            var rentals = await _rentalService.GetAllRentalsAsync();
            return Ok(ApiResponse<IEnumerable<RentalResponse>>.Ok(rentals, "Rentals retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving rentals.", e.Message));
        }
    }

    [HttpGet("my-rentals")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RentalResponse>>>> GetMyRentals()
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(ApiResponse<object>.Error(401, "Invalid token user ID."));

            var rentals = await _rentalService.GetMyRentalsAsync(userId);
            return Ok(ApiResponse<IEnumerable<RentalResponse>>.Ok(rentals, "Your rentals retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving your rentals.", e.Message));
        }
    }

    [HttpPost]
    [Authorize] 
    public async Task<ActionResult<ApiResponse<RentalResponse>>> Create([FromBody] CreateRentalRequest request)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized(ApiResponse<object>.Error(401, "Invalid token user ID."));

            int? employeeId = null;

            if (role == "Employee" || role == "Admin")
            {
                employeeId = userId;
            }
            else 
            {
                var tokenEmail = User.FindFirstValue(ClaimTypes.Email);
                if (request.CustomerEmail != tokenEmail)
                {
                    return Forbid("You cannot make a booking on behalf of another email address.");
                }
            }

            var rentalResponse = await _rentalService.CreateRentalAsync(request, employeeId);
            
            return CreatedAtAction(nameof(GetById), new { id = rentalResponse.Id }, ApiResponse<RentalResponse>.Ok(rentalResponse, "Rental created successfully."));
        }
        catch (InvalidOperationException e) 
        {
            return BadRequest(ApiResponse<object>.Error(400, e.Message));
        }
        catch (ArgumentException e) 
        {
            return BadRequest(ApiResponse<object>.Error(400, e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "An error occurred while processing the rental.", e.Message));
        }
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RentalResponse>>> GetById(int id)
    {
        try
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
            {
                return NotFound(ApiResponse<object>.NotFound($"Rental with ID {id} not found."));
            }
            
            // Basic authorization check: if user is a customer, they can only view their own rentals
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Customer")
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdString, out int userId) && rental.CustomerId != userId)
                {
                    return Forbid("You are not authorized to view this rental.");
                }
            }

            return Ok(ApiResponse<RentalResponse>.Ok(rental, "Rental retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "An error occurred while fetching the rental.", e.Message));
        }
    }

    [HttpGet("vehicle/{vehicleId:int}/unavailable-dates")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<DateRangeResponse>>>> GetUnavailableDates(int vehicleId)
    {
        try
        {
            var dates = await _rentalService.GetUnavailableDatesAsync(vehicleId);
            return Ok(ApiResponse<IEnumerable<DateRangeResponse>>.Ok(dates, "Unavailable dates retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "An error occurred while fetching dates.", e.Message));
        }
    }
}