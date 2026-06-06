using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
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

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if(customer == null) return false;

        await _customerRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        return _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException($"Customer with ID {id} does not exists.");

        _mapper.Map(request, customer);

        await _customerRepository.UpdateAsync(customer, id);

        return _mapper.Map<CustomerResponse>(customer);
    }
}