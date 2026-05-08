namespace LuxGarage.API.Models;
/// <summary>
/// Represents a vehicle price in the LuxGarage system, containing properties for the price's ID, associated offer, 
/// validity period, and price per day. This class serves as a data model for vehicle prices in the application, allowing for the storage
/// and retrieval of vehicle price information, including the details of the price and its association with a specific offer,
/// as well as the validity period for which the price is applicable. This class enables the application to manage and display 
/// vehicle price information effectively.
/// </summary>
public class VehiclePrice
{
    public int Id { get; set; }

    public int OfferId { get; set; }
    public Offer Offer { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
    public decimal PricePerDay { get; set; }
}