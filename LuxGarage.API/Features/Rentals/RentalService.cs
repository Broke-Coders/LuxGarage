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
            .Include(o => o.Vehicle)
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
            Vehicle = offer.Vehicle,
            Customer = customer, 
            EmployeeId = employeeId,
            StartingTime = request.StartDate,
            AppointedReturnTime = request.EndDate,
            Status = RentalStatus.ReservedWaitingForPayment, 
            VehiclePriceAtBooking = currentPricePerDay, 
            TotalPrice = vehicleTotal + insuranceTotal,
            RentalInsurances = rentalInsurances
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return _mapper.Map<RentalResponse>(rental);
    }

    public async Task<List<DateRangeResponse>> GetUnavailableDatesAsync(int vehicleId)
    {
        var rentals = await _context.Rentals
            .Where(r => r.VehicleId == vehicleId && r.Status != RentalStatus.Cancelled)
            .Select(r => new DateRangeResponse
            {
                StartDate = r.StartingTime,
                EndDate = r.AppointedReturnTime
            })
            .ToListAsync();

        return rentals;
    }

    public async Task<IEnumerable<RentalResponse>> GetAllRentalsAsync()
    {
        var rentals = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .OrderByDescending(r => r.StartingTime)
            .ToListAsync();
            
        return _mapper.Map<IEnumerable<RentalResponse>>(rentals);
    }

    public async Task<IEnumerable<RentalResponse>> GetMyRentalsAsync(int customerId)
    {
        var rentals = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.StartingTime)
            .ToListAsync();
            
        return _mapper.Map<IEnumerable<RentalResponse>>(rentals);
    }

    public async Task<RentalResponse?> GetRentalByIdAsync(int id)
    {
        var rental = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.Id == id);
            
        return rental == null ? null : _mapper.Map<RentalResponse>(rental);
    }

    public async Task<RentalResponse> CancelRentalAsync(int id, int userId, string? role)
    {
        var rental = await _context.Rentals
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental is null)
            throw new KeyNotFoundException($"Rental with ID {id} not found.");

        var isStaff = role == UserRole.Employee.ToString() || role == UserRole.Admin.ToString();
        if (!isStaff && rental.CustomerId != userId)
            throw new UnauthorizedAccessException("You are not authorized to cancel this rental.");

        if (rental.Status is RentalStatus.Cancelled or RentalStatus.Completed or RentalStatus.Active)
            throw new InvalidOperationException("This rental cannot be cancelled.");

        rental.Status = RentalStatus.Cancelled;
        await _context.SaveChangesAsync();

        return _mapper.Map<RentalResponse>(rental);
    }

    public async Task<RentalResponse> AcceptRentalAsync(int id, int userId, string? role)
    {
        var rental = await _context.Rentals
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental is null)
            throw new KeyNotFoundException($"Rental with ID {id} not found.");

        var isStaff = role == UserRole.Employee.ToString() || role == UserRole.Admin.ToString();
        if (!isStaff)
            throw new UnauthorizedAccessException("You are not authorized to accept this rental.");

        if (rental.Status is RentalStatus.Cancelled or RentalStatus.Completed or RentalStatus.Active)
            throw new InvalidOperationException("This rental cannot be accepted in its current state.");

        rental.Status = RentalStatus.Active;
        await _context.SaveChangesAsync();

        return _mapper.Map<RentalResponse>(rental);
    }
}
