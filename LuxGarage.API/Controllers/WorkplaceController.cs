using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkplaceController : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService;

        public WorkplaceController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

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
