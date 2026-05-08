using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
/// <summary>
/// Represents a vehicle in the LuxGarage system, containing properties for the vehicle's ID, associated brand, model, image, horsepower, 
/// license plate, mileage, year, body type, color, and collections of rentals and images. 
/// This class serves as a data model for vehicles in the application, allowing for the storage and retrieval of vehicle information, 
/// including the details of the vehicle's specifications and its rental history. This class also includes a collection of rentals and images
/// associated with the vehicle, enabling the application to manage and display the vehicle's rental history and images effectively. 
/// </summary>
public class Vehicle
{
    public int Id { get; set; }

    public int VehicleBrandId { get; set; } 
    public VehicleBrand VehicleBrand{ get; set; } = null!;

    public int VehicleModelId { get; set; }
    public VehicleModel VehicleModel { get; set; } = null!;

    public int VehicleImageId { get; set; }

    public decimal Horsepower { get; set; }
    public string LicensePlate { get; set; } = null!;
    public int Mileage { get; set; }
    public int year { get; set; }
    
    public int VehicleBodyId { get; set; }
    public VehicleBody VehicleBody { get; set; } = null!;

    public int VehicleColorId { get; set; }
    public VehicleColor VehicleColor { get; set; } = null!;

    public List<Rental> Rentals {get;} = new List<Rental>();
    public List<VehicleImage> Images { get; set; } = new List<VehicleImage>();
}
