using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers
{
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
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initializes a new instance of the EmployeeController class with the specified employee service.
        /// </summary>
        /// <param name="employeeService">The employee service to be used for handling employee-related operations.</param>
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        /// <returns>A list of employee responses wrapped in an ApiResponse object.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponse>>>> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.Ok(employees,
                    "All employees retrieved successfully."));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while retrieving employees.",
                        e.Message));
            }
        }

        /// <summary>
        /// Retrieves a specific employee by their ID. Returns 200 OK if found, 400 Bad Request for invalid ID,
        /// and 404 Not Found if no employee exists with the provided ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee response wrapped in an ApiResponse object.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Employee ID must be greater than 0."));
                }

                var employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} was not found."));
                }

                return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee found."));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while retrieving employee.",
                        e.Message));
            }
        }

        /// <summary>
        /// Updates an existing employee with the provided data.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="request">The update request containing the new employee data.</param>
        /// <returns>The updated employee response wrapped in an ApiResponse object.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Update(int id, UpdateEmployeeRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Employee ID must be greater than 0."));
                }

                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Data is required."));
                }

                var employee = await _employeeService.UpdateAsync(id, request);
                if (employee == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} was not found."));
                }

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
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while updating employee.",
                        e.Message));
            }
        }

        /// <summary>
        /// Deletes an employee by their ID. Returns 200 OK if deletion is successful, 400 Bad Request for invalid ID,
        /// and 404 Not Found if no employee exists with the provided ID.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>A response indicating the result of the deletion operation.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Employee ID must be greater than 0."));
                }

                var success = await _employeeService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Delete failed. Employee with ID {id} was not found."));
                }

                return Ok(ApiResponse<object>.NoContent("Employee removed successfully."));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while deleting employee.",
                        e.Message));
            }
        }


        /// <summary>
        /// Changes the password of an employee with the provided ID and new password data. Returns 200 OK if successful,
        /// 400 Bad Request for invalid input, and 404 Not Found if no employee exists with the provided ID.
        /// </summary>
        /// <param name="id">The ID of the employee whose password to change.</param>
        /// <param name="request">The request containing the new password.</param>
        /// <returns>A response indicating the result of the password change operation.</returns>
        [HttpPatch("{id:int}/change-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Employee ID must be greater than 0."));
                }

                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Password change data is required."));
                }

                var success = await _employeeService.ChangePasswordAsync(id, request);
                if (!success)
                {
                    return NotFound(
                        ApiResponse<object>.NotFound($"Cannot change password. Employee with ID {id} was not found."));
                }

                return Ok(ApiResponse<object>.NoContent("Password changed successfully"));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while changing password.",
                        e.Message));
            }
        }


    }
}
