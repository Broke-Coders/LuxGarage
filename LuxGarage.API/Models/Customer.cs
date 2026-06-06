namespace LuxGarage.API.Models;

/// <summary>
/// Represents a customer in the LuxGarage system, containing properties for the customer's ID, email, borrow counter, 
/// and a collection of rentals associated with the customer. This class serves as a data model for customers in
/// the application, allowing for the storage and retrieval of customer information and their rental history.
/// </summary>
public class Customer
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string LicenseNumber { get; set; }
    public required string Email { get; set; }

    public int BorrowCounter { get; set; } = 0;

    public ICollection<Rental> Rentals { get; } = new List<Rental>();
}