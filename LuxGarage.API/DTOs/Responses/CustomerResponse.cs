namespace LuxGarage.API.DTOs.Responses;

/// <summary>
/// Data transfer object representing a customer's profile information returned by the API.
/// </summary>
public class CustomerResponse
{
    /// <summary>Gets or sets the customer's unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the customer's first name.</summary>
    public required string FirstName { get; set; }
    /// <summary>Gets or sets the customer's last name.</summary>
    public required string LastName { get; set; }
    /// <summary>Gets or sets the customer's email address.</summary>
    public required string Email { get; set; }
    /// <summary>Gets or sets the customer's phone number.</summary>
    public required string PhoneNumber { get; set; }
    /// <summary>Gets or sets the customer's driver's license number.</summary>
    public required string LicenseNumber { get; set; }
    /// <summary>Gets or sets the total number of rentals associated with this customer.</summary>
    public int BorrowCounter { get; set; }
}