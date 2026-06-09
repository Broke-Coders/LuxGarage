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
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetAll()
    {
        // TO DO
        return Ok(new List<RentalResponse>()); 
    }

    [HttpGet("my-rentals")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetMyRentals()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!int.TryParse(userIdString, out int userId))
            return Unauthorized("Invalid token user ID.");

        return Ok(new List<RentalResponse>());
    }

    [HttpPost]
    [Authorize] 
    public async Task<ActionResult<RentalResponse>> Create([FromBody] CreateRentalRequest request)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            
            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized();

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
            
            return CreatedAtAction(nameof(GetById), new { id = rentalResponse.Id }, rentalResponse);
        }
        catch (InvalidOperationException e) 
        {
            return BadRequest(new { Message = e.Message });
        }
        catch (ArgumentException e) 
        {
            return BadRequest(new { Message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = "An error occurred while processing the rental.", Details = e.Message });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<RentalResponse>> GetById(int id)
    {
        // TO DO
        return Ok(); 
    }
}