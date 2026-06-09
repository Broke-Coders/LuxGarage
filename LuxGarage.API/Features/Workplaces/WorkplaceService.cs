using AutoMapper;
using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Workplaces;

public class WorkplaceService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public WorkplaceService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkplaceResponse>> GetAllAsync()
    {
        var workplaces = await _context.Workplaces.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<WorkplaceResponse>>(workplaces);
    }

    public async Task<WorkplaceResponse?> GetByIdAsync(int id)
    {
        var workplace = await _context.Workplaces.FindAsync(id);
        return workplace == null ? null : _mapper.Map<WorkplaceResponse>(workplace);
    }

    public async Task<WorkplaceResponse> CreateAsync(ChangeWorkplaceRequest request)
    {
        var workplace = new Models.Workplace // Zakładam ścieżkę encji, w razie potrzeby dostosuj using
        {
            Country = request.Country,
            City = request.City,
            Street = request.Street,
            BuildingNumber = request.BuildingNumber
        };

        await _context.Workplaces.AddAsync(workplace);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<WorkplaceResponse>(workplace);
    }

    public async Task<WorkplaceResponse?> UpdateAsync(int id, ChangeWorkplaceRequest request)
    {
        var workplace = await _context.Workplaces.FindAsync(id);
        if (workplace == null) return null;

        workplace.Country = request.Country;
        workplace.City = request.City;
        workplace.Street = request.Street;
        workplace.BuildingNumber = request.BuildingNumber;

        await _context.SaveChangesAsync();
        return _mapper.Map<WorkplaceResponse>(workplace);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // ExecuteDeleteAsync to optymalna metoda EF Core 7+, która usuwa bez pobierania wiersza z bazy
        var rowsDeleted = await _context.Workplaces.Where(w => w.Id == id).ExecuteDeleteAsync();
        return rowsDeleted > 0;
    }
}