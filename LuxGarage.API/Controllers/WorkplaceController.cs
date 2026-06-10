using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Workplaces;
using Microsoft.AspNetCore.Authorization;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Controller responsible for handling workplace-related endpoints, including retrieving all workplaces,
/// retrieving a workplace by ID, creating a new workplace, updating an existing workplace, and deleting a workplace.
/// </summary>
/// <remarks>
/// The WorkplaceController provides endpoints for managing workplaces, utilizing the IWorkplaceService to perform the
/// necessary business logic. The controller uses DTOs (Data Transfer Objects) to facilitate data serialization and deserialization
/// between the service layer and the client, ensuring that the data is properly structured.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class WorkplaceController : ControllerBase
{
    private readonly WorkplaceService _workplaceService;

    /// <summary>
    /// Initializes a new instance of the WorkplaceController class, 
    /// injecting the WorkplaceService to handle business logic related to workplaces.
    /// </summary>
    /// <param name="workplaceService">The WorkplaceService instance to use for workplace-related operations.</param>
    public WorkplaceController(WorkplaceService workplaceService)
    {
        _workplaceService = workplaceService;
    }

    /// <summary>
    /// Retrieves a list of all workplaces.
    /// </summary>
    /// <returns>A list of all workplaces.</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        try
        {
            var workplaces = await _workplaceService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<WorkplaceResponse>>.Ok(workplaces, "Workplaces retrieved."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving workplaces.", e.Message));
        }
    }

    /// <summary>
    /// Retrieves a specific workplace by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the workplace to retrieve.</param>
    /// <returns>The workplace details if found, otherwise null.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            var workplace = await _workplaceService.GetByIdAsync(id);
            if (workplace == null)
                return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} not found."));

            return Ok(ApiResponse<WorkplaceResponse>.Ok(workplace, "Workplace retrieved."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving workplace.", e.Message));
        }
    }


    /// <summary>
    /// Creates a new workplace based on the provided request data.
    /// </summary>
    /// <param name="request">The request DTO containing the data for the new workplace.</param>
    /// <returns>The details of the newly created workplace.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")] // Tylko Admin tworzy oddziały
    public async Task<ActionResult> Create([FromBody] ChangeWorkplaceRequest request)
    {
        try
        {
            var workplace = await _workplaceService.CreateAsync(request);
            // CreatedAtAction standardowo obsługuje zwrot HTTP 201
            return CreatedAtAction(nameof(GetById), new { id = workplace.Id }, 
                ApiResponse<WorkplaceResponse>.CreatedAt(workplace, "Workplace created."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error creating workplace.", e.Message));
        }
    }

    /// <summary>
    /// Updates an existing workplace based on the provided request data.
    /// </summary>
    /// <param name="request">The request DTO containing the updated data for the workplace.</param>
    /// <param name="id">The unique identifier of the workplace to update.</param>
    /// <returns>The details of the updated workplace.</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult> Update(int id, [FromBody] ChangeWorkplaceRequest request)
    {
        try
        {
            var workplace = await _workplaceService.UpdateAsync(id, request);
            if (workplace == null)
                return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} not found."));

            return Ok(ApiResponse<WorkplaceResponse>.Ok(workplace, "Workplace updated."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error updating workplace.", e.Message));
        }
    }

    /// <summary>
    /// Deletes a workplace by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the workplace to delete.</param>
    /// <returns>A response indicating the outcome of the delete operation.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await _workplaceService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} not found."));

            return Ok(ApiResponse<object>.NoContent("Workplace deleted."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error deleting workplace.", e.Message));
        }
    }
}