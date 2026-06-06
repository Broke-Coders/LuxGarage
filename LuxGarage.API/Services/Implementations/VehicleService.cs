using AutoMapper;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Services.Interfaces;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Services.Implementations;

/// <summary>
/// Implements the vehicle service for the LuxGarage API, providing methods for managing vehicle data, 
/// including retrieval, creation, and sorting of vehicles, while ensuring proper validation 
/// and mapping of data transfer objects to the underlying data models.
/// </summary>
public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleBodyRepository _vehicleBodyRepository;
    private readonly IVehicleColorRepository _vehicleColorRepository;
    private readonly IVehicleBrandRepository _vehicleBrandRepository;
    private readonly IVehicleImageRepository _vehicleImageRepository;
    private readonly IVehicleModelRepository _vehicleModelRepository;
    private readonly IVehiclePriceRepository _vehiclePriceRepository;

    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the VehicleService class with the specified vehicle repository and mapper.
    /// </summary>
    /// <param name="vehicleRepository">The vehicle repository to use.</param>
    /// <param name="mapper">The mapper to use.</param>
    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper, IVehicleBodyRepository vehicleBodyRepository,
                          IVehicleBrandRepository vehicleBrandRepository, IVehicleColorRepository vehicleColorRepository,
                          IVehicleImageRepository vehicleImageRepository, IVehicleModelRepository vehicleModelRepository,
                          IVehiclePriceRepository vehiclePriceRepository)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
        _vehicleBodyRepository = vehicleBodyRepository;
        _vehicleBrandRepository = vehicleBrandRepository;
        _vehicleColorRepository = vehicleColorRepository;
        _vehicleImageRepository = vehicleImageRepository;
        _vehicleModelRepository = vehicleModelRepository;
        _vehiclePriceRepository = vehiclePriceRepository;
    }

    /// <summary>
    /// Retrieves all vehicles from the repository, applies sorting based on the provided request parameters,
    /// and maps the resulting list of vehicles to a list of VehicleListItemResponse DTOs
    /// </summary>
    /// <param name="request">The request containing the sorting parameters.</param>
    /// <returns>The list of vehicle items.</returns>
    public async Task<List<VehicleListItemResponse>> GetAllAsync(GetVehiclesRequest request)
    {
        var query = _vehicleRepository.GetAllQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(v => v.VehicleBrand.Name.Contains(request.SearchTerm)
                                || v.VehicleModel.Name.Contains(request.SearchTerm));
        }

        if (request.BrandId.HasValue)
        {
            query = query.Where(v => v.VehicleBrandId == request.BrandId);
        }

        if (request.BodyTypeId.HasValue)
        {
            query = query.Where(v => v.VehicleBodyId == request.BodyTypeId);
        }

        if (request.YearFrom.HasValue)
        {
            query = query.Where(v => v.year >= request.YearFrom);
        }

        if (request.YearTo.HasValue)
        {
            query = query.Where(v => v.year <= request.YearTo);
        }

        /*
        // show only avalible cars
        query = query.Where(v => v.VehicleStatus.Name == StatusType.Available);
        */
        query = ApplySortingQueryable(query, request);

        var vehicles = await query.ToListAsync();

        return _mapper.Map<List<VehicleListItemResponse>>(vehicles);
    }

    /// <summary>
    /// Retrieves a vehicle by its ID from the repository and maps it to a VehicleDetailsResponse DTO,
    /// returning null if the vehicle is not found.
    /// </summary>
    /// <param name="id">The ID of the vehicle to retrieve.</param>
    /// <returns>The VehicleDetailsResponse DTO, or null if the vehicle is not found.</returns>
    public async Task<VehicleDetailsResponse?> GetByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);

        if (vehicle is null)
            return null;

        return _mapper.Map<VehicleDetailsResponse>(vehicle);
    }

    /// <summary>
    /// Creates a new vehicle based on the provided request, ensuring that a vehicle with the same license plate does not already exist.
    /// </summary>
    /// <param name="request">The request containing the new vehicle information.</param>
    /// <returns>The VehicleDetailsResponse DTO for the created vehicle.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a vehicle with the specified license plate already exists.</exception>
    public async Task<VehicleDetailsResponse> CreateAsync(CreateVehicleRequest request)
    {
        var existingVehicle = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate);

        if (existingVehicle is not null)
            throw new InvalidOperationException("Vehicle with this license plate already exists.");

        var vehicle = _mapper.Map<Vehicle>(request);

        await _vehicleRepository.AddAsync(vehicle);
        
        return _mapper.Map<VehicleDetailsResponse>(vehicle);
    }

    /// <summary>
    /// Applies sorting to the list of vehicles based on the sorting parameters provided in the request,
    /// allowing sorting by brand, model, body type, color, horsepower, mileage, or license plate in either ascending or descending order.
    /// </summary>
    /// <param name="vehicles">The list of vehicles to sort.</param>
    /// <param name="request">The request containing the sorting parameters.</param>
    /// <returns>The sorted list of vehicles.</returns>
    private static List<Vehicle> ApplySorting(List<Vehicle> vehicles, GetVehiclesRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            ("brand", false) => vehicles.OrderBy(v => v.VehicleBrand.Name).ToList(),
            ("brand", true) => vehicles.OrderByDescending(v => v.VehicleBrand.Name).ToList(),

            ("model", false) => vehicles.OrderBy(v => v.VehicleModel.Name).ToList(),
            ("model", true) => vehicles.OrderByDescending(v => v.VehicleModel.Name).ToList(),

            ("body", false) => vehicles.OrderBy(v => v.VehicleBody.Name).ToList(),
            ("body", true) => vehicles.OrderByDescending(v => v.VehicleBody.Name).ToList(),

            ("color", false) => vehicles.OrderBy(v => v.VehicleColor.Name).ToList(),
            ("color", true) => vehicles.OrderByDescending(v => v.VehicleColor.Name).ToList(),

            ("horsepower", false) => vehicles.OrderBy(v => v.Horsepower).ToList(),
            ("horsepower", true) => vehicles.OrderByDescending(v => v.Horsepower).ToList(),

            ("mileage", false) => vehicles.OrderBy(v => v.Mileage).ToList(),
            ("mileage", true) => vehicles.OrderByDescending(v => v.Mileage).ToList(),

            ("licenseplate", false) => vehicles.OrderBy(v => v.LicensePlate).ToList(),
            ("licenseplate", true) => vehicles.OrderByDescending(v => v.LicensePlate).ToList(),

            _ => request.Descending
                ? vehicles.OrderByDescending(v => v.Id).ToList()
                : vehicles.OrderBy(v => v.Id).ToList()
        };
    }

    private static IQueryable<Vehicle> ApplySortingQueryable(IQueryable<Vehicle> vehicles, GetVehiclesRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            ("brand", false) => vehicles.OrderBy(v => v.VehicleBrand.Name),
            ("brand", true) => vehicles.OrderByDescending(v => v.VehicleBrand.Name),

            ("model", false) => vehicles.OrderBy(v => v.VehicleModel.Name),
            ("model", true) => vehicles.OrderByDescending(v => v.VehicleModel.Name),

            ("body", false) => vehicles.OrderBy(v => v.VehicleBody.Name),
            ("body", true) => vehicles.OrderByDescending(v => v.VehicleBody.Name),

            ("color", false) => vehicles.OrderBy(v => v.VehicleColor.Name),
            ("color", true) => vehicles.OrderByDescending(v => v.VehicleColor.Name),

            ("horsepower", false) => vehicles.OrderBy(v => v.Horsepower),
            ("horsepower", true) => vehicles.OrderByDescending(v => v.Horsepower),

            ("mileage", false) => vehicles.OrderBy(v => v.Mileage),
            ("mileage", true) => vehicles.OrderByDescending(v => v.Mileage),

            ("licenseplate", false) => vehicles.OrderBy(v => v.LicensePlate),
            ("licenseplate", true) => vehicles.OrderByDescending(v => v.LicensePlate),

            _ => request.Descending
                ? vehicles.OrderByDescending(v => v.Id)
                : vehicles.OrderBy(v => v.Id)
        };
    }

    public async Task<VehicleDetailsResponse> UpdateAsync(int id, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
                      ?? throw new KeyNotFoundException($"Vehicle with ID {id} doest not exists.");

        var body = await _vehicleBodyRepository.GetByIdAsync(vehicle.VehicleBodyId)
                      ?? throw new KeyNotFoundException($"Vehicle body with ID {vehicle.VehicleBodyId} doest not exists.");

        var color = await _vehicleColorRepository.GetByIdAsync(vehicle.VehicleColorId)
                      ?? throw new KeyNotFoundException($"Vehicle color with ID {vehicle.VehicleColorId} doest not exists.");

        var brand = await _vehicleBrandRepository.GetByIdAsync(vehicle.VehicleBrandId)
                      ?? throw new KeyNotFoundException($"Vehicle brand with ID {vehicle.VehicleBrandId} doest not exists.");

        var image = await _vehicleImageRepository.GetByIdAsync(vehicle.VehicleImageId)
                      ?? throw new KeyNotFoundException($"Vehicle image with ID {vehicle.VehicleImageId} doest not exists.");

        var model = await _vehicleModelRepository.GetByIdAsync(vehicle.VehicleModelId)
                      ?? throw new KeyNotFoundException($"Vehicle model with ID {vehicle.VehicleModelId} doest not exists.");

        /*
        var vehicle = await _vehiclePriceRepository.GetByIdAsync(vehicle.)
                      ?? throw new KeyNotFoundException($"Vehicle price with ID {id} doest not exists.");
        */

        _mapper.Map(request, vehicle);

        await _vehicleRepository.UpdateAsync(vehicle, id);

        return _mapper.Map<VehicleDetailsResponse>(vehicle);

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle == null) return false;

        await _vehicleRepository.DeleteAsync(id);
        return true;
    }
}