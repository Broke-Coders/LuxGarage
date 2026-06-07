namespace LuxGarage.API.DTOs.Responses;

/// <summary>
/// Data transfer object representing the details of a vehicle rental transaction.
/// </summary>
public class RentalResponse
{
    /// <summary>Gets or sets the unique identifier for the rental record.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the start date and time of the rental.</summary>
    public DateTime StartingTime { get; set; }
    /// <summary>Gets or sets the scheduled return date and time.</summary>
    public DateTime AppointedReturnTime { get; set; }
    /// <summary>Gets or sets the total price calculated for the rental period.</summary>
    public decimal TotalPrice { get; set; }
    /// <summary>Gets or sets the unique identifier of the rented vehicle.</summary>
    public int VehicleId { get; set; }
    /// <summary>Gets or sets the unique identifier of the customer who rented the vehicle.</summary>
    public int CustomerId { get; set; }
    /// <summary>Gets or sets the unique identifier of the employee who processed the rental.</summary>
    public int EmployeeId { get; set; }
    /// <summary>Gets or sets the current status of the rental.</summary>
    public string Status { get; set; } = null!;
}