using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Users;
using Microsoft.AspNetCore.Authorization;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Provides administrative and operational endpoints for managing customer profiles within the LuxGarage system.
/// Supports full CRUD operations, enabling staff to register, update, and manage customer data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    /// <param name="customerService">The service for customer business logic.</param>
     public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Retrieves a list of all customers.
    /// </summary>
    /// <returns>A wrapped collection of customer response DTOs.</returns>
    [HttpGet]
    [Authorize(Roles = "Employee, Admin")] 
    public async Task<ActionResult> GetAll()
    {
        try
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CustomerResponse>>.Ok(customers, "All customers retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving customers.", e.Message));
        }
    }

    /// <summary>
    /// Retrieves a specific customer by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>A wrapped customer response DTO.</returns>
     [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        try
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(ApiResponse<object>.NotFound($"Customer with ID {id} not found."));

            return Ok(ApiResponse<CustomerResponse>.Ok(customer, "Customer found."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error retrieving customer.", e.Message));
        }
    }
        
    /// <summary>
    /// Updates an existing customer's profile details.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="request">The partial update request data.</param>
    /// <returns>The updated customer details.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.UpdateAsync(id, request);
            return Ok(ApiResponse<CustomerResponse>.Ok(customer, "Customer updated successfully."));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(ApiResponse<object>.NotFound(e.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error updating customer.", e.Message));
        }
    }

    /// <summary>
    /// Removes a customer from the system.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    /// <returns>A confirmation of the deletion.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await _customerService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.NotFound($"Customer with ID {id} not found."));

            return Ok(ApiResponse<object>.NoContent("Customer removed successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error deleting customer.", e.Message));
        }
    }
}