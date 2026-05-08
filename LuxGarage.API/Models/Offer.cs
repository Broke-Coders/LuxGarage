namespace LuxGarage.API.Models;

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