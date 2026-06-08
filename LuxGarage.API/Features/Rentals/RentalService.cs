using AutoMapper;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.API.Features.Rentals;

public class RentalService
{
    private readonly RentalContext _context;
    private readonly DynamicPricingEngine _pricingEngine;
    private readonly IMapper _mapper;

    public RentalService(RentalContext context, DynamicPricingEngine pricingEngine, IMapper mapper)
    {
        _context = context;
        _pricingEngine = pricingEngine;
        _mapper = mapper;
    }

    public async Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request, int? employeeId = null)
    {
        bool isUnavailable = await _context.Rentals
            .Where(r => r.VehicleId == request.VehicleId && r.Status != RentalStatus.Cancelled)
            .AnyAsync(r => request.StartDate < r.AppointedReturnTime && request.EndDate > r.StartingTime);

        if (isUnavailable)
            throw new InvalidOperationException("Car is not available in the selected date range.");

        var offer = await _context.Offers
            .Include(o => o.Prices)
            .FirstOrDefaultAsync(o => o.VehicleId == request.VehicleId);
            
        if (offer == null) throw new InvalidOperationException("No active offer for this vehicle.");
        
        var currentPricePerDay = offer.Prices.FirstOrDefault(p => p.ValidTo == null)?.PricePerDay 
                                 ?? throw new InvalidOperationException("Price not defined.");

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.CustomerEmail);
        if (customer == null)
        {
            customer = new Customer
            {
                Email = request.CustomerEmail,
                PasswordHash = "", 
                FirstName = request.FirstName ?? "Unknown",
                LastName = request.LastName ?? "Unknown",
                PhoneNumber = request.PhoneNumber ?? "N/A",
                LicenseNumber = request.LicenseNumber ?? "N/A",
                Role = UserRole.Customer
            };
            _context.Customers.Add(customer);
        }

        var pricingContext = new RentalPricingContext
        {
            BasePricePerDay = currentPricePerDay,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        decimal vehicleTotal = _pricingEngine.CalculateTotal(pricingContext);

        decimal insuranceTotal = 0;
        var rentalInsurances = new List<RentalInsurance>();
        
        if (request.SelectedInsuranceIds != null && request.SelectedInsuranceIds.Any())
        {
            var insurances = await _context.Insurances
                .Where(i => request.SelectedInsuranceIds.Contains(i.Id) && i.IsActive)
                .ToListAsync();

            foreach (var ins in insurances)
            {
                insuranceTotal += ins.PricePerDay * pricingContext.TotalDays;
                
                rentalInsurances.Add(new RentalInsurance
                {
                    InsuranceId = ins.Id,
                    PriceAtBooking = ins.PricePerDay
                });
            }
        }

        var rental = new Rental
        {
            VehicleId = request.VehicleId,
            Customer = customer, 
            EmployeeId = employeeId,
            StartingTime = request.StartDate,
            AppointedReturnTime = request.EndDate,
            Status = RentalStatus.Pending, 
            VehiclePriceAtBooking = currentPricePerDay, 
            TotalPrice = vehicleTotal + insuranceTotal,
            RentalInsurances = rentalInsurances
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return _mapper.Map<RentalResponse>(rental);
    }
}