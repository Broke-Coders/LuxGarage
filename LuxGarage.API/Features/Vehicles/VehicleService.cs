using AutoMapper;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Vehicles;

public class VehicleService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;
    private readonly VehicleImageService _imageService;

    public VehicleService(RentalContext context, IMapper mapper, VehicleImageService imageService)
    {
        _context = context;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<List<VehicleResponse>> GetAllAsync(GetVehiclesRequest request)
    {
        var query = _context.Vehicles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(v => v.Brand.ToLower().Contains(term) || v.Model.ToLower().Contains(term));
        }

        if (request.BodyType.HasValue)
            query = query.Where(v => v.BodyType == request.BodyType.Value);

        if (request.Status.HasValue)
            query = query.Where(v => v.Status == request.Status.Value);

        if (request.YearFrom.HasValue)
            query = query.Where(v => v.Year >= request.YearFrom);

        if (request.YearTo.HasValue)
            query = query.Where(v => v.Year <= request.YearTo);

        query = ApplySorting(query, request);

        var vehicles = await query.ToListAsync();
        return _mapper.Map<List<VehicleResponse>>(vehicles);
    }

    public async Task<VehicleResponse?> GetByIdAsync(int id)
    {
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);

        return vehicle is null ? null : _mapper.Map<VehicleResponse>(vehicle);
    }

    public async Task<VehicleResponse> CreateAsync(CreateVehicleRequest request)
    {
        var exists = await _context.Vehicles.AnyAsync(v => v.LicensePlate == request.LicensePlate);
        if (exists)
            throw new InvalidOperationException("Vehicle with this license plate already exists.");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var vehicle = new Vehicle
            {
                Brand        = request.Brand,
                Model        = request.Model,
                LicensePlate = request.LicensePlate,
                EngineName   = request.EngineName,
                Year         = request.Year,
                Horsepower   = request.Horsepower,
                Mileage      = request.Mileage,
                SpeedToHundred = request.ToHundred,
                EngineType   = request.EngineType,
                BodyType     = request.BodyType,
                Color        = request.Color,
                Status       = request.Status,
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            if (request.Images is { Count: > 0 })
            {
                await _imageService.UploadImagesAsync(new UploadVehicleImagesRequest
                {
                    VehicleId         = vehicle.Id,
                    Images            = request.Images,
                    PrimaryImageIndex = 0
                });
            }

            await transaction.CommitAsync();
            return _mapper.Map<VehicleResponse>(vehicle);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VehicleResponse> UpdateAsync(int id, UpdateVehicleRequest request)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehicle with ID {id} does not exist.");

        vehicle.Mileage = request.Mileage;
        vehicle.Status = request.Status;

        await _context.SaveChangesAsync();

        return _mapper.Map<VehicleResponse>(vehicle);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == id);
        if (!vehicleExists) return false;

        await _imageService.DeleteVehicleDirAsync(id);

        var rowsDeleted = await _context.Vehicles
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        return rowsDeleted > 0;
    }

    private static IQueryable<Vehicle> ApplySorting(IQueryable<Vehicle> query, GetVehiclesRequest request)
    {
        var sortBy = request.SortBy?.Trim().ToLower();

        return (sortBy, request.Descending) switch
        {
            ("brand", false) => query.OrderBy(v => v.Brand),
            ("brand", true) => query.OrderByDescending(v => v.Brand),
            ("model", false) => query.OrderBy(v => v.Model),
            ("model", true) => query.OrderByDescending(v => v.Model),
            ("horsepower", false) => query.OrderBy(v => v.Horsepower),
            ("horsepower", true) => query.OrderByDescending(v => v.Horsepower),
            ("mileage", false) => query.OrderBy(v => v.Mileage),
            ("mileage", true) => query.OrderByDescending(v => v.Mileage),
            _ => request.Descending
                ? query.OrderByDescending(v => v.Id)
                : query.OrderBy(v => v.Id)
        };
    }
}