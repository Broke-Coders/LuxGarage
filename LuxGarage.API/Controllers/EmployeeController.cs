using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Users;
using Microsoft.AspNetCore.Authorization;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Controller responsible for handling employee-related endpoints, including CRUD operations and password changes.
/// </summary>
/// <remarks>
/// The EmployeeController provides endpoints for managing employees, utilizing the IEmployeeService to perform the necessary business logic.
/// The controller includes proper error handling and returns appropriate HTTP status codes based on the outcome of the operations, 
/// such as 200 OK for successful retrieval and updates, 400 Bad Request for invalid input, 
/// 404 Not Found for non-existent resources, and 500 Internal Server Error for unexpected exceptions.
/// Additionally, it handles specific exceptions like KeyNotFoundException and InvalidOperationException 
/// to provide more informative responses to clients in case of errors.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    /// <summary>
    /// Initializes a new instance of the EmployeeController class with the specified employee service.
    /// </summary>
    /// <param name="employeeService">The employee service to be used for handling employee-related operations.</param>
    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>A list of employee responses wrapped in an ApiResponse object.</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        try
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.Ok(employees, "All employees retrieved."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving employees.", e.Message));
        }
    }

    /// <summary>
    /// Retrieves a specific employee by their ID. Returns 200 OK if found, 400 Bad Request for invalid ID,
    /// and 404 Not Found if no employee exists with the provided ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The employee response wrapped in an ApiResponse object.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} not found."));

            return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee found."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving employee.", e.Message));
        }
    }

    /// <summary>
    /// Updates an existing employee with the provided data.
    /// </summary>
    /// <param name="id">The ID of the employee to update.</param>
    /// <param name="request">The update request containing the new employee data.</param>
    /// <returns>The updated employee response wrapped in an ApiResponse object.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        try
        {
            var employee = await _employeeService.UpdateAsync(id, request);
            return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee updated successfully."));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse<object>.NotFound(e.Message));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(ApiResponse<object>.BadRequest(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error updating employee.", e.Message));
        }
    }


    /// <summary>
    /// Deletes an employee by their ID. Returns 200 OK if deletion is successful, 400 Bad Request for invalid ID,
    /// and 404 Not Found if no employee exists with the provided ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <returns>A response indicating the result of the deletion operation.</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await _employeeService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} not found."));

            return Ok(ApiResponse<object>.NoContent("Employee removed successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error deleting employee.", e.Message));
        }
    }

    [HttpGet("pending-employees")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingEmployees()
    {
        var pendingUsers = await _employeeService.GetPendingEmployeesAsync();

        return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.Ok(pendingUsers, "All employees retrieved successfully."));
    }

    [HttpGet("approve-employee/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveEmployee(int userId)
    {
        var success = await _employeeService.AcceptEmployeeRequest(userId);
        if (!success) 
            return NotFound(ApiResponse<object>.NotFound($"Employee with ID {userId} not found."));
        return Ok(ApiResponse<object>.NoContent("Employee accepted."));
    }
}
