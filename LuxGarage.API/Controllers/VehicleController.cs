using LuxGarage.API.Features.Vehicles;
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
    private readonly VehicleService _vehicleService;

    /// <summary>
    /// Initializes a new instance of the VehiclesController class, injecting the IVehicleService to handle business logic related to vehicles.
    /// </summary>
    /// <param name="vehicleService">The IVehicleService instance to use for vehicle-related operations.</param>
    public VehiclesController(VehicleService vehicleService)
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
    public async Task<ActionResult<ApiResponse<List<VehicleResponse>>>> GetAll([FromQuery] GetVehiclesRequest request)
    {
        var vehicles = await _vehicleService.GetAllAsync(request);
        return Ok(vehicles);
    }

    /// <summary>
    /// Retrieves a specific vehicle by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to retrieve.</param>
    /// <returns>The vehicle details if found, otherwise null.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleResponse>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        
        if (vehicle is null)
            return NotFound();

        return Ok(vehicle);
    }

    /// <summary>
    /// Creates a new vehicle based on the provided request data. 
    /// </summary>
    /// <param name="request">The request DTO containing the data for the new vehicle.</param>
    /// <returns>The details of the newly created vehicle.</returns>
    [HttpPost]
    public async Task<ActionResult<VehicleResponse>> Create([FromForm] CreateVehicleRequest request)
    {
        try
        {
            var response = await _vehicleService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleResponse>> Update(int id, [FromBody] UpdateVehicleRequest request)
    {
        try
        {
            var response = await _vehicleService.UpdateAsync(id, request);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _vehicleService.DeleteAsync(id);
        
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}