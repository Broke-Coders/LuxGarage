namespace LuxGarage.API.Models;
public class VehiclePrice
{
    public int Id { get; set; }

    public int OfferId { get; set; }
    public Offer Offer { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
    public decimal PricePerDay { get; set; }
}