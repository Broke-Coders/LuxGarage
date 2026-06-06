using System;

namespace LuxGarage.API.DTOs.Requests.Vehicle;

public class UpdateVehicleRequest
{
    public int? VehicleBrandId { get; set; }
    public int? VehicleModelId { get; set; }
    public int? VehicleBodyId { get; set; }
    public int? VehicleColorId { get; set; }
    public decimal? Horsepower { get; set; }
    public string? LicensePlate { get; set; }
    public int? Mileage { get; set; }
}
