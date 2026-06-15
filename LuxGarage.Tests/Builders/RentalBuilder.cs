using LuxGarage.API.Models;
using LuxGarage.API.Features.Rentals;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Rental instances.
/// </summary>
public class RentalBuilder
{
    private int _vehicleId = 1;
    private int _customerId = 1;
    private int? _employeeId = 1;
    private DateTime _startingTime = DateTime.UtcNow;
    private DateTime _appointedReturnTime = DateTime.UtcNow.AddDays(7);
    private RentalStatus _status = RentalStatus.Pending;
    private decimal _vehiclePriceAtBooking = 100.0m;
    private decimal _totalPrice = 700.0m;

    public RentalBuilder WithVehicleId(int vehicleId)
    {
        _vehicleId = vehicleId;
        return this;
    }

    public RentalBuilder WithCustomerId(int customerId)
    {
        _customerId = customerId;
        return this;
    }

    public RentalBuilder WithEmployeeId(int? employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public RentalBuilder WithStartingTime(DateTime startingTime)
    {
        _startingTime = startingTime;
        return this;
    }

    public RentalBuilder WithAppointedReturnTime(DateTime appointedReturnTime)
    {
        _appointedReturnTime = appointedReturnTime;
        return this;
    }

    public RentalBuilder WithStatus(RentalStatus status)
    {
        _status = status;
        return this;
    }

    public RentalBuilder WithVehiclePriceAtBooking(decimal vehiclePriceAtBooking)
    {
        _vehiclePriceAtBooking = vehiclePriceAtBooking;
        return this;
    }

    public RentalBuilder WithTotalPrice(decimal totalPrice)
    {
        _totalPrice = totalPrice;
        return this;
    }

    public Rental Build() => new Rental
    {
        VehicleId = _vehicleId,
        CustomerId = _customerId,
        EmployeeId = _employeeId,
        StartingTime = _startingTime,
        AppointedReturnTime = _appointedReturnTime,
        Status = _status,
        VehiclePriceAtBooking = _vehiclePriceAtBooking,
        TotalPrice = _totalPrice
    };
}
