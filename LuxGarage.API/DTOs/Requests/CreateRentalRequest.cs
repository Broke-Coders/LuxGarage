namespace LuxGarage.API.DTOs.Requests;

/// <summary>
/// Data transfer object for initiating a new vehicle rental.
/// Contains both rental period details and customer information for "on-the-fly" registration.
/// </summary>
public class CreateRentalRequest
{
    /// <summary>Gets or sets the ID of the vehicle to be rented.</summary>
    public int VehicleId { get; set; }
    /// <summary>Gets or sets the email of the customer. Used as a unique identifier for finding or creating the customer.</summary>
    public required string CustomerEmail { get; set; }
    /// <summary>Gets or sets the intended start date and time of the rental.</summary>
    public DateTime StartDate { get; set; }
    /// <summary>Gets or sets the intended end date and time of the rental.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Gets or sets the customer's first name (required for new customers).</summary>
    public string? FirstName { get; set; }
    /// <summary>Gets or sets the customer's last name (required for new customers).</summary>
    public string? LastName { get; set; }
    /// <summary>Gets or sets the customer's phone number (required for new customers).</summary>
    public string? PhoneNumber { get; set; }
    /// <summary>Gets or sets the customer's driver's license number (required for new customers).</summary>
    public string? LicenseNumber { get; set; }
}