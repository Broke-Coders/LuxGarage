namespace LuxGarage.API.Models;

/// <summary>
/// Represents an offer in the LuxGarage system, containing properties for the offer's ID, 
/// associated vehicle, vehicle status, description, publication date, and a collection of vehicle prices. 
/// This class serves as a data model for offers in the application, allowing for the storage and retrieval of offer information, 
/// including the details of the vehicle being offered and its pricing history.
/// </summary>

public class Offer
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int VehicleStatusId { get; set; }
    public VehicleStatus Status { get; set; } = null!;

    public string? Description { get; set; }
    public DateTime PublicationDate { get; set; }

    public List<VehiclePrice> Prices { get; set; } = new List<VehiclePrice>();
}