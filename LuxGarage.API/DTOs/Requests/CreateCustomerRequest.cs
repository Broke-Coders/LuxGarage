namespace LuxGarage.API.DTOs.Requests;

/// <summary>
/// Data transfer object used for registering a new customer in the system.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>Gets or sets the customer's first name.</summary>
    public required string FirstName { get; set; }
    /// <summary>Gets or sets the customer's last name.</summary>
    public required string LastName { get; set; }
    /// <summary>Gets or sets the customer's email address.</summary>
    public required string Email { get; set; }
    /// <summary>Gets or sets the customer's contact phone number.</summary>
    public required string PhoneNumber { get; set; }
    /// <summary>Gets or sets the customer's driver's license number.</summary>
    public required string LicenseNumber { get; set; }

}