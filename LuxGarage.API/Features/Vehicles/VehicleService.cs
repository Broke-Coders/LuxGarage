using AutoMapper;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Vehicles;

public class VehicleService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public VehicleService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

        var vehicle = new Vehicle
        {
            Brand = request.Brand,
            Model = request.Model,
            LicensePlate = request.LicensePlate,
            Year = request.Year,
            Horsepower = request.Horsepower,
            Mileage = request.Mileage,
            BodyType = request.BodyType,
            Color = request.Color,
            Status = request.Status
        };

        // TODO: Zapis zdjęć (jeśli request.Images != null) 
        // np. wywołując metodę UploadImage(IFormFile) lub wstrzykując serwis chmurowy/S3.

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<VehicleResponse>(vehicle);
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
        var rowsDeleted = await _context.Vehicles.Where(v => v.Id == id).ExecuteDeleteAsync();
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