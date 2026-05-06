namespace LuxGarage.API.DTOs.Responses.Vehicle;

public class VehicleListItemResponse
{
    public int Id { get; set; }
    public string BrandName { get; set; } = null!;

    public string ModelName { get; set; }
    public string BodyName { get; set; } = null!;
    public string ColorName { get; set; } = null!;
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }
    public string LicensePlate { get; set; } = null!;
}