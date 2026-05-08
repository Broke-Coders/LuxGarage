using System;

namespace LuxGarage.API.DTOs.Responses;

/// <summary>
/// DTO for vehicle response, containing the vehicle's ID, brand, mileage, license plate, horsepower, body type, 
/// and color to be returned in API responses when retrieving information about a specific vehicle or a list of vehicles, 
/// providing a comprehensive overview of the vehicle's details.
/// </summary>
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
