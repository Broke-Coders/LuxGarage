using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VehicleListItemResponse>>>> GetAll([FromQuery] GetVehiclesRequest request)
    {
        var vehicles = await _vehicleService.GetAllAsync(request);
        return Ok(ApiResponse<List<VehicleListItemResponse>>.Ok(vehicles,
        "All employees retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<VehicleDetailsResponse?> GetById(int id)
    {
        return await _vehicleService.GetByIdAsync(id);
    }


    [HttpPost]
    public async Task<VehicleDetailsResponse> Create([FromBody] CreateVehicleRequest request)
    {
        return await _vehicleService.CreateAsync(request);
    }
}