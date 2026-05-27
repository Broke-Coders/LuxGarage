using LuxGarage.API.DTOs.VehicleImage.Responses;

namespace LuxGarage.API.DTOs.Responses.Offer;

public class OfferDetailsResponse
{
    public int Id { get; set; }
    public int VehicleId { get; set; }

    public string BrandName { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string BodyName { get; set; } = null!;
    public string ColorName { get; set; } = null!;
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }
    public string LicensePlate { get; set; } = null!;

    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateOnly? AvailableFrom { get; set; }
    public DateOnly? AvailableTo { get; set; }
    public string? Description { get; set; }

    public List<VehicleImageResponse> Images { get; set; } = new();
}