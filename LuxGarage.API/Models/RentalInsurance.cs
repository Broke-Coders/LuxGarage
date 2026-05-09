namespace LuxGarage.API.Models;

/// <summary>
/// Represents a rental insurance in the LuxGarage system, containing properties for the rental insurance's ID, associated rental, 
/// and associated insurance. This class serves as a data model for rental insurances in the application, allowing for 
/// the storage and retrieval of rental insurance information, including the details of the rental and 
/// insurance associated with the rental insurance.
/// </summary>
public class RentalInsurance
{
    public int Id { get; set; }

    public int RentalId { get; set; }

    public Rental Rental { get; set; } = null!;
    public int InsuranceId { get; set; }
    public Insurance Insurance { get; set; } = null!;
}