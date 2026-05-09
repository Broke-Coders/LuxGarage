namespace LuxGarage.API.Models;

/// <summary>
/// Represents a vehicle model in the LuxGarage system, containing properties for the vehicle model's ID, name, 
/// associated vehicle brand, and a collection of vehicles associated with the model.
/// This class serves as a data model for vehicle models in the application, 
/// allowing for the storage and retrieval of vehicle model information, 
/// including the details of the vehicle model and its associated vehicles.
/// </summary>
public class VehicleModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int VehicleBrandId { get; set; }
    public VehicleBrand VehicleBrand { get; set; } = null!;
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}