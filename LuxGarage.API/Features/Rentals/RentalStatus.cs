namespace LuxGarage.API.Features.Rentals;

public enum RentalStatus
{
    Pending = 1,
    ReservedWaitingForPayment = 2,
    Active = 3,
    Completed = 4,
    Cancelled = 5
}