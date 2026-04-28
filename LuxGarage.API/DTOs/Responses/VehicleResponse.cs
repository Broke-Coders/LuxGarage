using System;

namespace LuxGarage.API.DTOs.Responses;

public class VehicleResponse
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public int Mileage { get; set; } 
    public string LicensePlate { get; set; } = string.Empty;
    public decimal Horsepower { get; set;}
    public string Body { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
