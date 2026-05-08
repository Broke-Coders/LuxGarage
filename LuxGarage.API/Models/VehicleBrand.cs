using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;
/// <summary>
/// Represents a vehicle brand in the LuxGarage system, containing properties for the vehicle brand's ID, name, 
/// and a collection of vehicles associated with the brand. This class serves as a data model for
/// vehicle brands in the application, allowing for the storage and retrieval of vehicle brand information, 
/// including the details of the vehicle brand and its associated vehicles. 
/// This class also includes a collection of vehicles associated with the brand,
/// enabling the application to manage and display the vehicle brand's vehicles effectively.
/// </summary>
public class VehicleBrand
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
    public ICollection<VehicleModel> VehicleModels { get; } = new List<VehicleModel>();
}
