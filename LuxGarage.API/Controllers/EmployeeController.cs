using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

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
