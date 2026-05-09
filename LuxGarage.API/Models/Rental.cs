namespace LuxGarage.API.Models;

/// <summary>
/// Represents a rental in the LuxGarage system, containing properties for the rental's ID, associated vehicle, 
/// starting time, appointed return time, real return time, associated customer, a collection of rental insurances, 
/// associated employee, and total price. This class serves as a data model for rentals in the application, 
/// allowing for the storage and retrieval of rental information, including the details of the vehicle being rented, 
/// the customer renting it, the employee handling the rental, and any insurances associated with the rental.
/// </summary>
public class Rental
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle {get; set;} = null!;

    public DateTime StartingTime { get; set; }

    public DateTime AppointedReturnTime { get; set; }

    public DateTime? RealReturnTime { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<RentalInsurance> RentalInsurances { get; } = new List<RentalInsurance>();

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public decimal TotalPrice { get; set; }
}