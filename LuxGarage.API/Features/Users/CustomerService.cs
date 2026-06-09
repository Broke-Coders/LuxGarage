using AutoMapper;
using LuxGarage.API.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Users;

public class CustomerService
{
    private readonly RentalContext _context;
    private readonly IMapper _mapper;

    public CustomerService(RentalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
    {
        var customers = await _context.Customers.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        return customer == null ? null : _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Customer with ID {id} does not exist.");

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.PhoneNumber = request.PhoneNumber;
        customer.LicenseNumber = request.LicenseNumber;

        await _context.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _context.Customers.Where(c => c.Id == id).ExecuteDeleteAsync();
        return deleted > 0;
    }
}