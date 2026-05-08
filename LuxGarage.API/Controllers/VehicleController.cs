using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Controller responsible for handling vehicle-related endpoints, including retrieving all vehicles, 
/// retrieving a vehicle by ID, and creating a new vehicle.
/// </summary> 
/// <remarks>
/// The VehiclesController provides endpoints for managing vehicles, utilizing the IVehicleService to perform the
/// necessary business logic. The controller uses DTOs (Data Transfer Objects) to facilitate data serialization and deserialization 
/// between the service layer and the client, ensuring that the data is properly structured. 
/// The controller includes proper error handling and returns appropriate HTTP status codes based on the outcome of the operations, 
/// such as 200 OK for successful retrievals, 201 Created for successful creation, and 400 Bad Request for invalid input. 
/// </remarks>

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    /// <summary>
    /// Initializes a new instance of the VehiclesController class, injecting the IVehicleService to handle business logic related to vehicles.
    /// </summary>
    /// <param name="vehicleService">The IVehicleService instance to use for vehicle-related operations.</param>
    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>
    /// Retrieves a list of all vehicles based on the provided query parameters. 
    /// The endpoint supports filtering, sorting, and pagination through the GetVehiclesRequest DTO.
    /// </summary>
    /// <param name="request">The request DTO containing query parameters for filtering, sorting, and pagination.</param>
    /// <returns>A list of vehicles matching the criteria.</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VehicleListItemResponse>>>> GetAll([FromQuery] GetVehiclesRequest request)
    {
        var vehicles = await _vehicleService.GetAllAsync(request);
        return Ok(ApiResponse<List<VehicleListItemResponse>>.Ok(vehicles,
        "All employees retrieved successfully."));
    }

    /// <summary>
    /// Retrieves a specific vehicle by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to retrieve.</param>
    /// <returns>The vehicle details if found, otherwise null.</returns>
    [HttpGet("{id:int}")]
    public async Task<VehicleDetailsResponse?> GetById(int id)
    {
        return await _vehicleService.GetByIdAsync(id);
    }

    /// <summary>
    /// Creates a new vehicle based on the provided request data. 
    /// </summary>
    /// <param name="request">The request DTO containing the data for the new vehicle.</param>
    /// <returns>The details of the newly created vehicle.</returns>
    [HttpPost]
    public async Task<VehicleDetailsResponse> Create([FromBody] CreateVehicleRequest request)
    {
        return await _vehicleService.CreateAsync(request);
    }
}