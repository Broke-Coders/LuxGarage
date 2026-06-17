using AutoMapper;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Insurances;

public class InsuranceService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public InsuranceService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<InsuranceResponse>> GetAllAsync()
    {
        var insurances = await _context.Insurances
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .ToListAsync();

        return _mapper.Map<List<InsuranceResponse>>(insurances);
    }

    public async Task<InsuranceResponse?> GetByIdAsync(int id)
    {
        var insurance = await _context.Insurances
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        return insurance is null ? null : _mapper.Map<InsuranceResponse>(insurance);
    }

    public async Task<InsuranceResponse> CreateAsync(CreateInsuranceRequest request)
    {
        var exists = await _context.Insurances
            .AnyAsync(i => i.Name == request.Name);

        if (exists)
            throw new InvalidOperationException($"Insurance with name '{request.Name}' already exists.");

        var insurance = new Insurance
        {
            Name        = request.Name,
            PricePerDay = request.PricePerDay,
            IsActive    = request.IsActive,
        };

        _context.Insurances.Add(insurance);
        await _context.SaveChangesAsync();

        return _mapper.Map<InsuranceResponse>(insurance);
    }

    public async Task<InsuranceResponse> UpdateAsync(int id, UpdateInsuranceRequest request)
    {
        var insurance = await _context.Insurances.FindAsync(id);

        if (insurance is null)
            throw new KeyNotFoundException($"Insurance with ID {id} does not exist.");

        var nameConflict = await _context.Insurances
            .AnyAsync(i => i.Name == request.Name && i.Id != id);

        if (nameConflict)
            throw new InvalidOperationException($"Insurance with name '{request.Name}' already exists.");

        insurance.Name        = request.Name;
        insurance.PricePerDay = request.PricePerDay;
        insurance.IsActive    = request.IsActive;

        await _context.SaveChangesAsync();

        return _mapper.Map<InsuranceResponse>(insurance);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var isInUse = await _context.RentalInsurances
            .AnyAsync(ri => ri.InsuranceId == id);

        if (isInUse)
            throw new InvalidOperationException("Cannot delete insurance that is assigned to existing rentals.");

        var rows = await _context.Insurances
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync();

        return rows > 0;
    }
}