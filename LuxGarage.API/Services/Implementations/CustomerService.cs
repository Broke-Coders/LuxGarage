using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations;

/// <summary>
/// Implements the customer service for the LuxGarage API, providing core logic for 
/// customer management including creation, profile updates, and data retrieval.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="customerRepository">The repository for customer data access.</param>
    /// <param name="mapper">The AutoMapper instance for DTO conversions.</param>
    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new customer, ensuring email uniqueness before persistence.
    /// </summary>
    /// <param name="request">The customer creation request data.</param>
    /// <returns>A response DTO representing the newly created customer.</returns>
    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(request.Email);

        if (existingCustomer is not null)
        {
            throw new InvalidOperationException("Customer with this email already exists.");
        }

        var customer = _mapper.Map<Customer>(request);

        await _customerRepository.AddAsync(customer);

        return _mapper.Map<CustomerResponse>(customer);
    }

    /// <summary>
    /// Deletes a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the customer to remove.</param>
    /// <returns>True if the customer was found and deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if(customer == null) return false;

        await _customerRepository.DeleteAsync(id);
        return true;
    }

    /// <summary>
    /// Retrieves a list of all registered customers.
    /// </summary>
    /// <returns>A list of customer response DTOs.</returns>
    public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    /// <summary>
    /// Retrieves a specific customer by their ID.
    /// </summary>
    /// <param name="id">The ID of the customer to find.</param>
    /// <returns>The customer response DTO if found; otherwise, null.</returns>
    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        return _mapper.Map<CustomerResponse>(customer);
    }

    /// <summary>
    /// Updates an existing customer's information using partial update logic.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="request">The update request containing modified fields.</param>
    /// <returns>The updated customer response DTO.</returns>
    public async Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException($"Customer with ID {id} does not exists.");

        _mapper.Map(request, customer);

        await _customerRepository.UpdateAsync(customer, id);

        return _mapper.Map<CustomerResponse>(customer);
    }
}