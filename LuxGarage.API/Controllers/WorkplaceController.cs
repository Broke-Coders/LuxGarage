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


    }
}
