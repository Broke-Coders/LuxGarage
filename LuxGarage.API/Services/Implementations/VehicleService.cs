using AutoMapper;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Services.Interfaces;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<List<VehicleListItemResponse>> GetAllAsync(GetVehiclesRequest request)
    {
        var vehicles = await _vehicleRepository.GetAllAsync();

        vehicles = ApplySorting(vehicles, request);

        return _mapper.Map<List<VehicleListItemResponse>>(vehicles);
    }

    public async Task<VehicleDetailsResponse?> GetByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);

        if (vehicle is null)
            return null;

        return _mapper.Map<VehicleDetailsResponse>(vehicle);
    }

    public async Task<VehicleDetailsResponse> CreateAsync(CreateVehicleRequest request)
    {
        var existingVehicle = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate);

        if (existingVehicle is not null)
            throw new InvalidOperationException("Vehicle with this license plate already exists.");

        var vehicle = _mapper.Map<Vehicle>(request);

        return _mapper.Map<VehicleDetailsResponse>(vehicle);
        
    }

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

}