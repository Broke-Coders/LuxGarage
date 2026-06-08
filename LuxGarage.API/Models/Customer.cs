namespace LuxGarage.API.Models;

/// <summary>
/// Represents a customer in the LuxGarage system, containing comprehensive profile information including 
/// contact details, driver's license data, and rental history tracking. This class serves as the 
/// primary data model for customer management and rental associations.
/// </summary>
public class Customer : User
{
    public required string PhoneNumber { get; set; }
    public required string LicenseNumber { get; set; }
    public int BorrowCounter { get; set; } = 0;
    public ICollection<Rental> Rentals { get; } = new List<Rental>();
}