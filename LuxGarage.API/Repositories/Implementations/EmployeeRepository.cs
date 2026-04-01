using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly RentalContext _context;

    public EmployeeRepository(RentalContext context)
    {
        this._context = context;
    }

    public async Task<Employee?> GetByIdAsync(int id) 
        => await _context.Employees.FindAsync(id);
    
    public async Task AddAsync(Employee worker)
    {
        await _context.Employees.AddAsync(worker);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee worker, int id)
    {
        Employee? oldWorker = await _context.Employees.FindAsync(id);

        oldWorker = worker;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        
        if (employee == null)
        {
            Console.WriteLine("employee with given id not found");
            return;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}