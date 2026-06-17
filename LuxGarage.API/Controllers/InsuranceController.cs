using LuxGarage.API.Features.Insurances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsurancesController : ControllerBase
{
    private readonly InsuranceService _insuranceService;

    public InsurancesController(InsuranceService insuranceService)
    {
        _insuranceService = insuranceService;
    }

    /// <summary>
    /// Retrieves all insurance options. Available to all authenticated users (needed during booking).
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<InsuranceResponse>>> GetAll()
    {
        var insurances = await _insuranceService.GetAllAsync();
        return Ok(insurances);
    }

    /// <summary>
    /// Retrieves a single insurance option by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<InsuranceResponse>> GetById(int id)
    {
        var insurance = await _insuranceService.GetByIdAsync(id);

        if (insurance is null)
            return NotFound();

        return Ok(insurance);
    }

    /// <summary>
    /// Creates a new insurance option. Admin only.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InsuranceResponse>> Create([FromBody] CreateInsuranceRequest request)
    {
        try
        {
            var response = await _insuranceService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing insurance option. Admin only.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InsuranceResponse>> Update(int id, [FromBody] UpdateInsuranceRequest request)
    {
        try
        {
            var response = await _insuranceService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an insurance option. Admin only. Fails if insurance is assigned to any rental.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _insuranceService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }
}