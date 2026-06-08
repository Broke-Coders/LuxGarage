namespace LuxGarage.API.Models;

/// <summary>
/// Represents a customer in the LuxGarage system, containing comprehensive profile information including 
/// contact details, driver's license data, and rental history tracking. This class serves as the 
/// primary data model for customer management and rental associations.
/// </summary>
public class Customer : User
{
    /// <summary>Gets or sets the unique identifier for the customer.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the customer's first name.</summary>
    public required string FirstName { get; set; }

    /// <summary>Gets or sets the customer's last name.</summary>
    public required string LastName { get; set; }

    /// <summary>Gets or sets the customer's contact phone number.</summary>
    public required string PhoneNumber { get; set; }

    /// <summary>Gets or sets the customer's driver's license number for validation and legal requirements.</summary>
    public required string LicenseNumber { get; set; }

    /// <summary>Gets or sets the customer's email address, used for communication and as a unique identifier.</summary>
    public required string Email { get; set; }

    /// <summary>Gets or sets a counter representing the total number of rentals completed by the customer.</summary>
    public int BorrowCounter { get; set; } = 0;

    /// <summary>Gets the collection of rentals associated with this customer.</summary>
    public ICollection<Rental> Rentals { get; } = new List<Rental>();
}