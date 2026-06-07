using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers;

/// <summary>
/// Provides administrative and operational endpoints for managing customer profiles within the LuxGarage system.
/// Supports full CRUD operations, enabling staff to register, update, and manage customer data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    /// <param name="customerService">The service for customer business logic.</param>
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Retrieves a list of all customers.
    /// </summary>
    /// <returns>A wrapped collection of customer response DTOs.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponse>>>> GetAll()
    {
        try
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CustomerResponse>>.Ok(customers,
                "All customers retrieved successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500,
                ApiResponse<object>.Error(500, "An unexpected error occured while retrieving customers.",
                    e.Message));
        }
    }

    /// <summary>
    /// Retrieves a specific customer by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>A wrapped customer response DTO.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Customer ID must be greater than 0."));
                }

                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Customer with ID {id} was not found."));
                }

                return Ok(ApiResponse<CustomerResponse>.Ok(customer, "Customer found."));
            }
            catch (Exception e)
            {
                return StatusCode(500,
                    ApiResponse<object>.Error(500, "An unexpected error occured while retrieving customer.",
                        e.Message));
            }
        }
        
    /// <summary>
    /// Updates an existing customer's profile details.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="request">The partial update request data.</param>
    /// <returns>The updated customer details.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> Update(int id, UpdateCustomerRequest request)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Customer ID must be greater than 0."));
            }
            if (request == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Data is required."));
            }
            var customer = await _customerService.UpdateAsync(id, request);
            if (customer == null)
            {
                return NotFound(ApiResponse<object>.NotFound($"Customer with ID {id} was not found."));
            }
            return Ok(ApiResponse<CustomerResponse>.Ok(customer, "Customer updated successfully."));
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
                ApiResponse<object>.Error(500, "An unexpected error occured while updating customer.",
                    e.Message));
        }
    }


    /// <summary>
    /// Removes a customer from the system.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    /// <returns>A confirmation of the deletion.</returns>
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
                return BadRequest(ApiResponse<object>.BadRequest("Customer ID must be greater than 0."));
            }
            var success = await _customerService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(ApiResponse<object>.NotFound($"Delete failed. Customer with ID {id} was not found."));
            }
            return Ok(ApiResponse<object>.NoContent("Customer removed successfully."));
        }
        catch (Exception e)
        {
            return StatusCode(500,
                ApiResponse<object>.Error(500, "An unexpected error occured while deleting customer.",
                    e.Message));
        }
    }

    /// <summary>
    /// Registers a new customer in the LuxGarage system.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>The newly created customer details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> Create(CreateCustomerRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Customer data is required"));
            }
            var customer = await _customerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id },
                ApiResponse<CustomerResponse>.CreatedAt(customer, "Customer created successfully."));
        }
        catch (Exception e)
        {
            var errorResponse =
                ApiResponse<object>.Error(500, "An unexpected error occured while creating customer.", e.Message);
            return StatusCode(500, errorResponse);
        }
    }
}