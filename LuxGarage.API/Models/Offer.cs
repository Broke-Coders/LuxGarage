namespace LuxGarage.API.Models;

public class Offer
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    
    public int VehiclePriceId { get; set; }
    public VehiclePrice Price { get; set; }

    public int VehicleStatusId { get; set; }
    public VehicleStatus Status { get; set; }
    
    public string? Description { get; set; }
    public DateTime PublicationDate { get; set; }
}