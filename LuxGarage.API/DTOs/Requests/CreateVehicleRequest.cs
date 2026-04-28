namespace LuxGarage.API.DTOs.Requests.Vehicle;

public class CreateVehicleRequest
{
    public int VehicleBrandId { get; set; }
    public int VehicleBodyId { get; set; }
    public int VehicleColorId { get; set; }
    public decimal Horsepower { get; set; }
    public string LicensePlate { get; set; } = null!;
    public int Mileage { get; set; }
}