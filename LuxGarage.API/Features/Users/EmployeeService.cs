using AutoMapper;
using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Users;

public class EmployeeService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Workplace)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
    }

    public async Task<EmployeeResponse?> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Workplace)
            .FirstOrDefaultAsync(e => e.Id == id);

        return employee == null ? null : _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<EmployeeResponse> UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await _context.Employees.FindAsync(id)
            ?? throw new KeyNotFoundException($"Employee with ID {id} does not exist.");

        var branchExists = await _context.Workplaces.AnyAsync(b => b.Id == request.WorkplaceId);
        if (!branchExists)
            throw new InvalidOperationException($"Workplace with ID {request.WorkplaceId} does not exist.");

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Role = request.Role;
        employee.WorkplaceId = request.WorkplaceId;
        employee.IsActive = request.IsActive;

        await _context.SaveChangesAsync();
        
        await _context.Entry(employee).Reference(e => e.Workplace).LoadAsync();
        
        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _context.Employees.Where(e => e.Id == id).ExecuteDeleteAsync();
        return deleted > 0;
    }
    public async Task<IEnumerable<EmployeeResponse>> GetPendingEmployeesAsync()
    {
        var pendingUsers = await _context.Employees
                        .Where(e => e.Status == EmployeeStatus.Pending)
                        .AsNoTracking()
                        .ToListAsync();

        return _mapper.Map<IEnumerable<EmployeeResponse>>(pendingUsers);
    }

    public async Task<bool> ResolveEmployeeRequest(int id, EmployeeStatus newStatus)
    {
        var employee = await _context.Employees.FindAsync(id)
            ?? throw new KeyNotFoundException($"Employee with ID {id} does not exist.");

        employee.Status = newStatus;
        employee.Role = UserRole.Employee;

        await _context.SaveChangesAsync();

        return employee.Role == UserRole.Employee;
    }
}