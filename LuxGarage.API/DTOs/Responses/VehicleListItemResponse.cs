namespace LuxGarage.API.DTOs.Responses.Vehicle;

/// <summary>
/// DTO for vehicle list item response, containing the vehicle's ID, brand, model, body type, color, horsepower, 
/// mileage, and license plate to be returned in API responses when retrieving a list of vehicles, providing a summary of 
/// each vehicle's information for display in lists or overviews.
/// </summary>
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