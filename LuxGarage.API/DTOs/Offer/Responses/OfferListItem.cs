namespace LuxGarage.API.DTOs.Responses.Offer;

public class OfferListItemResponse
{
    public int Id { get; set; }
    public int VehicleId { get; set; }

    // dane pojazdu
    public string BrandName { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string BodyName { get; set; } = null!;
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }

    // dane oferty
    public bool IsAvailable { get; set; }
    public DateOnly? AvailableFrom { get; set; }
    public DateOnly? AvailableTo { get; set; }

    public string? PrimaryImageUrl { get; set; }
}