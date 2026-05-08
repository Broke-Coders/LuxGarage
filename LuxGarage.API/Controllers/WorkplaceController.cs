using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers
{
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
        private readonly IWorkplaceService _workplaceService;

        /// <summary>
        /// Initializes a new instance of the WorkplaceController class, 
        /// injecting the IWorkplaceService to handle business logic related to workplaces.
        /// </summary>
        /// <param name="workplaceService">The IWorkplaceService instance to use for workplace-related operations.</param>
        public WorkplaceController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        /// <summary>
        /// Retrieves a list of all workplaces.
        /// </summary>
        /// <returns>A list of all workplaces.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkplaceResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkplaceResponse>>>> GetAll()
        {
            try
            {
                var workplaces = await _workplaceService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<WorkplaceResponse>>.Ok(workplaces,
                    "Workplaces retrieved successfully."));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while retrieving workplaces",
                        e.Message));
            }
        }

        /// <summary>
        /// Retrieves a specific workplace by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the workplace to retrieve.</param>
        /// <returns>The workplace details if found, otherwise null.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WorkplaceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WorkplaceResponse>>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Workplace ID must be greater than 0."));
                }
                var workplace = await _workplaceService.GetByIdAsync(id);
                if (workplace == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} not found."));
                }
                return Ok(ApiResponse<WorkplaceResponse>.Ok(workplace, "Workplace retrieved successfully."));

            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while retrieving workplace",
                        e.Message));
            }
        }

        /// <summary>
        /// Creates a new workplace based on the provided request data.
        /// </summary>
        /// <param name="request">The request DTO containing the data for the new workplace.</param>
        /// <returns>The details of the newly created workplace.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<WorkplaceResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WorkplaceResponse>>> Create(ChangeWorkplaceRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Workplace data is required"));
                }

                var workplace = await _workplaceService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = workplace.Id },
                    ApiResponse<WorkplaceResponse>.CreatedAt(workplace, "Workplace created successfully."));
            }
            catch (Exception e)
            {
                var errorResponse =
                    ApiResponse<object>.Error(500, "An unexpected error occured while creating workplace.", e.Message);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Updates an existing workplace based on the provided request data.
        /// </summary>
        /// <param name="request">The request DTO containing the updated data for the workplace.</param>
        /// <param name="id">The unique identifier of the workplace to update.</param>
        /// <returns>The details of the updated workplace.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WorkplaceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WorkplaceResponse>>> Update(ChangeWorkplaceRequest request, int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Workplace ID must be greater than 0."));
                }

                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Workplace data is required"));
                }

                var workplace = await _workplaceService.UpdateAsync(request, id);
                if (workplace == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} was not found."));
                }

                return Ok(ApiResponse<WorkplaceResponse>.Ok(workplace, "Workplace updated successfully"));
            }
            catch (Exception e)
            {
                var errorResponse =
                    ApiResponse<object>.Error(500, "An unexpected error occured while updating workplace.", e.Message);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Deletes a workplace by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the workplace to delete.</param>
        /// <returns>A response indicating the outcome of the delete operation.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Workplace ID must be greater than 0."));

                }

                var success = await _workplaceService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Workplace with ID {id} was not found."));
                }

                return Ok(ApiResponse<object>.NoContent("Workplace deleted successfully."));

            }
            catch (Exception e)
            {
                var errorResponse =
                    ApiResponse<object>.Error(500, "An unexpected error occured while deleting workplace.", e.Message);
                return StatusCode(500, errorResponse);
            }
        }


    }
}
