using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Controller for managing vehicle rentals.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    /// <summary>
    /// Retrieves all rentals.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RentalResponse>>>> GetAll()
    {
        var rentals = await _rentalService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<RentalResponse>>.Ok(rentals, "Rentals retrieved successfully."));
    }

    /// <summary>
    /// Retrieves a specific rental by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<RentalResponse>>> GetById(int id)
    {
        var rental = await _rentalService.GetByIdAsync(id);
        if (rental == null)
            return NotFound(ApiResponse<object>.NotFound($"Rental with ID {id} not found."));

        return Ok(ApiResponse<RentalResponse>.Ok(rental, "Rental found."));
    }

    /// <summary>
    /// Creates a new rental.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RentalResponse>>> Create(CreateRentalRequest request)
    {
        // For now, using a hardcoded employee ID (simulating logged in user)
        var employeeId = 1;
        var rental = await _rentalService.CreateRentalAsync(request, employeeId);
        return CreatedAtAction(nameof(GetById), new { id = rental.Id }, 
            ApiResponse<RentalResponse>.CreatedAt(rental, "Rental created successfully."));
    }

    /// <summary>
    /// Updates an existing rental.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<RentalResponse>>> Update(int id, UpdateRentalRequest request)
    {
        try
        {
            var rental = await _rentalService.UpdateAsync(id, request);
            return Ok(ApiResponse<RentalResponse>.Ok(rental, "Rental updated successfully."));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse<object>.NotFound(e.Message));
        }
    }

    /// <summary>
    /// Deletes a rental.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var success = await _rentalService.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.NotFound($"Rental with ID {id} not found."));

        return Ok(ApiResponse<object>.NoContent("Rental deleted successfully."));
    }
}