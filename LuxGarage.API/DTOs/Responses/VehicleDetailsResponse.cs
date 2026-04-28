namespace LuxGarage.API.DTOs.Responses.Vehicle;

public class VehicleDetailsResponse
{
    public int Id { get; set; }
    public int VehicleBrandId { get; set; }
    public string BrandName { get; set; } = null!;
    public int VehicleBodyId { get; set; }
    public string BodyName { get; set; } = null!;
    public int VehicleColorId { get; set; }
    public string ColorName { get; set; } = null!;
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }
    public string LicensePlate { get; set; } = null!;
}