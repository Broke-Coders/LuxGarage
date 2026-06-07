using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations;

/// <summary>
/// Implements the core rental engine for the LuxGarage API, orchestrating the booking lifecycle,
/// pricing strategies (including long-term discounts), and vehicle availability management.
/// </summary>
public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehiclePriceRepository _priceRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RentalService"/> class with necessary repository dependencies.
    /// </summary>
    public RentalService(IRentalRepository rentalRepository, IMapper mapper,
        IVehicleRepository vehicleRepository, ICustomerRepository customerRepository,
        IVehiclePriceRepository vehiclePriceRepository)
    {
        _rentalRepository = rentalRepository;
        _mapper = mapper;
        _vehicleRepository = vehicleRepository;
        _customerRepository = customerRepository;
        _priceRepository = vehiclePriceRepository;
    }

    /// <summary>
    /// Calculates the total rental price, applying a 20% discount for long-term rentals (over 30 days).
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to determine the base daily rate.</param>
    /// <param name="start">Rental start date.</param>
    /// <param name="end">Rental end date.</param>
    /// <returns>The calculated total price.</returns>
    public async Task<decimal> CalculateTotalPriceAsync(int vehicleId, DateTime start, DateTime end)
    {
        var days = (end - start).Days;
        if (days < 1) throw new 
                            ArgumentException("The rental period must be at least one day.");

        decimal pricePerDay = await _priceRepository.GetCurrentPriceByVehicleIdAsync(vehicleId);
        
        decimal total = days * pricePerDay;

        if (days > 30)
        {
            total *= 0.8m;
        }

        return total;
    }

    /// <summary>
    /// Executes the rental booking process, including "on-the-fly" customer creation and availability final checks.
    /// </summary>
    /// <param name="request">The rental request details.</param>
    /// <param name="employeeId">The ID of the processing employee.</param>
    /// <returns>A finalized rental response.</returns>
    public async Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request, int employeeId)
    {   
        var customer = await _customerRepository.GetByEmailAsync(request.CustomerEmail);

        if (customer == null) 
        {
            customer = new Customer
            {
                Email = request.CustomerEmail,
                FirstName = request.FirstName ?? "Unknown",
                LastName = request.LastName ?? "Unknown",
                PhoneNumber = request.PhoneNumber ?? "N/A",
                LicenseNumber = request.LicenseNumber ?? "N/A"
            };
            await _customerRepository.AddAsync(customer);
        }

        var isAvalible = await IsVehicleAvailableAsync(request.VehicleId, request.StartDate, request.EndDate);
        if (!isAvalible) throw new InvalidOperationException("Car is not available");
        
        var totalPrice = await CalculateTotalPriceAsync(request.VehicleId, request.StartDate, request.EndDate);
        
        var rental = new Rental
        {
            CustomerId = customer.Id,
            VehicleId = request.VehicleId,
            StartingTime = request.StartDate,
            AppointedReturnTime = request.EndDate,
            TotalPrice = totalPrice,
            EmployeeId = employeeId
        };

        customer.BorrowCounter++;
        await _customerRepository.UpdateAsync(customer, customer.Id);

        await _rentalRepository.AddAsync(rental);

        return _mapper.Map<RentalResponse>(rental);
    }

    /// <summary>
    /// Determines vehicle availability by checking for overlapping rentals in the requested time frame.
    /// </summary>
    public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end)
    {
        var rentals = await _rentalRepository.GetByVehicleIdAsync(vehicleId);

        return !rentals.Any(r => start < r.AppointedReturnTime && end > r.StartingTime);
    }
}