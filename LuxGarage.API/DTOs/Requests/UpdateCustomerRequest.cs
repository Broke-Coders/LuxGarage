namespace LuxGarage.API.DTOs.Requests;

/// <summary>
/// Data transfer object for partially updating an existing customer's profile.
/// All fields are optional; only provided fields will be updated.
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>Gets or sets the updated first name.</summary>
    public string? FirstName { get; set; }
    /// <summary>Gets or sets the updated last name.</summary>
    public string? LastName { get; set; }
    /// <summary>Gets or sets the updated email address.</summary>
    public string? Email { get; set; }
    /// <summary>Gets or sets the updated contact phone number.</summary>
    public string? PhoneNumber { get; set; }
    /// <summary>Gets or sets the updated driver's license number.</summary>
    public string? LicenseNumber { get; set; }

}