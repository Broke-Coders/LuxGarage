using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories;

namespace LuxGarage.API.Services.Interfaces;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<List<VehicleListItemResponse>> GetAllAsync(GetVehiclesRequest request)
    {
        var vehicles = await _vehicleRepository.GetAllAsync();

        vehicles = ApplySorting(vehicles, request);

        return vehicles
            .Select(MapToVehicleListItemResponse)
            .ToList();
    }

    public async Task<VehicleDetailsResponse?> GetByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);

        if (vehicle is null)
            return null;

        return MapToVehicleDetailsResponse(vehicle);
    }

    public async Task<VehicleDetailsResponse> CreateAsync(CreateVehicleRequest request)
    {
        var existingVehicle = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate);

        if (existingVehicle is not null)
            throw new InvalidOperationException("Vehicle with this license plate already exists.");

        var vehicle = new Vehicle
        {
            VehicleBrandId = request.VehicleBrandId,
            VehicleBodyId = request.VehicleBodyId,
            VehicleColorId = request.VehicleColorId,
            Horsepower = request.Horsepower,
            LicensePlate = request.LicensePlate,
            Mileage = request.Mileage
        };

        await _vehicleRepository.AddAsync(vehicle);

        var createdVehicle = await _vehicleRepository.GetByLicensePlateAsync(vehicle.LicensePlate);

        if (createdVehicle is null)
            throw new InvalidOperationException("Created vehicle could not be loaded.");

        return MapToVehicleDetailsResponse(createdVehicle);
    }

    private static List<Vehicle> ApplySorting(List<Vehicle> vehicles, GetVehiclesRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            ("brand", false) => vehicles.OrderBy(v => v.VehicleBrand.Name).ToList(),
            ("brand", true) => vehicles.OrderByDescending(v => v.VehicleBrand.Name).ToList(),

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

    private static VehicleListItemResponse MapToVehicleListItemResponse(Vehicle vehicle)
    {
        return new VehicleListItemResponse
        {
            Id = vehicle.Id,
            BrandName = vehicle.VehicleBrand.Name,
            BodyName = vehicle.VehicleBody.Name,
            ColorName = vehicle.VehicleColor.Name,
            Horsepower = vehicle.Horsepower,
            Mileage = vehicle.Mileage,
            LicensePlate = vehicle.LicensePlate
        };
    }

    private static VehicleDetailsResponse MapToVehicleDetailsResponse(Vehicle vehicle)
    {
        return new VehicleDetailsResponse
        {
            Id = vehicle.Id,
            VehicleBrandId = vehicle.VehicleBrandId,
            BrandName = vehicle.VehicleBrand.Name,
            VehicleBodyId = vehicle.VehicleBodyId,
            BodyName = vehicle.VehicleBody.Name,
            VehicleColorId = vehicle.VehicleColorId,
            ColorName = vehicle.VehicleColor.Name,
            Horsepower = vehicle.Horsepower,
            Mileage = vehicle.Mileage,
            LicensePlate = vehicle.LicensePlate
        };
    }
}