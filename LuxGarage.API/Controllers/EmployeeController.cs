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
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponse>>>> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.Ok(employees,
                "All employees retrieved successfully."));
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} was not found."));
            }

            return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee found."));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Update(int id, UpdateEmployeeRequest request)
        {
            var employee = await _employeeService.UpdateAsync(id, request);
            if (employee == null)
            {
                return NotFound(ApiResponse<object>.NotFound($"Employee with ID {id} was not found."));
            }

            return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee updated successfully."));
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var success = await _employeeService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(ApiResponse<object>.NotFound($"Delete failed. Employee with ID {id} was not found."));
            }

            return Ok(ApiResponse<object>.NoContent("Employee removed successfully."));
        }


}
}
