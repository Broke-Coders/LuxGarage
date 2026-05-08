namespace LuxGarage.API.DTOs.Requests.Vehicle;

/// <summary>
/// DTO for creating a new vehicle, containing the necessary information such as brand, model, 
/// body type, color, horsepower, license plate, and mileage to be set for the new vehicle.
/// </summary>
public class CreateVehicleRequest
{
    public int VehicleBrandId { get; set; }
    public int VehicleModelId { get; set; }
    public int VehicleBodyId { get; set; }
    public int VehicleColorId { get; set; }
    public decimal Horsepower { get; set; }
    public string LicensePlate { get; set; } = null!;
    public int Mileage { get; set; }
}