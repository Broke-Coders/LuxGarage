using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Repositories.Implementations;

/// <summary>
/// Represents a repository for managing employee data in the LuxGarage API, providing methods for retrieving, adding, updating,
/// and deleting employee information from the database.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly RentalContext _context;

    /// <summary>
    /// Initializes a new instance of the EmployeeRepository class, providing the necessary context for accessing the database
    /// and performing operations on employee data.
    /// </summary>
    /// <param name="context">The database context for accessing employee data.</param>
    public EmployeeRepository(RentalContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// Retrieves all employees from the database, allowing for the retrieval of a list of employee 
    /// records along with their associated permissions and workplaces.
    /// </summary>
    /// <returns>A list of all employees.</returns>
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Permission)
            .Include(e => e.Workplace)
            .AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Retrieves an employee by their id from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the employee to retrieve.</param>
    /// <returns>The employee with the specified identifier, or null if not found.</returns>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Permission)
            .Include(e => e.Workplace)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    //=> await _context.Employees.FindAsync(id)

    /// <summary>
    /// Retrieves an employee by their login from the database, allowing for the retrieval of specific 
    /// employee information based on the provided login credentials.
    /// </summary>
    /// <param name="login">The login of the employee to retrieve.</param>
    /// <returns>The employee with the specified login, or null if not found.</returns>
    public async Task<Employee?> GetByLoginAsync(string login)
    {
        return await _context.Employees
            .Include(e => e.Permission)
            .Include(e => e.Workplace)
            .FirstOrDefaultAsync(e => e.Login == login);
    }

    /// <summary>
    /// Adds a new employee to the database.
    /// </summary>
    /// <param name="worker">The employee to add.</param>
    public async Task AddAsync(Employee worker)
    {
        await _context.Employees.AddAsync(worker);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing employee in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the employee to update.</param>
    /// <param name="worker">The updated employee information.</param>  
    public async Task UpdateAsync(int id, Employee worker)
    {
        Employee? oldWorker = await _context.Employees.FindAsync(id);

        if (oldWorker != null)
        {
            oldWorker.Login = worker.Login;
            oldWorker.WorkplaceId = worker.WorkplaceId;
            oldWorker.PermissionId = worker.PermissionId;

            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes an employee from the database based on their unique identifier, allowing for the removal of employee records from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the employee to delete.</param>
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