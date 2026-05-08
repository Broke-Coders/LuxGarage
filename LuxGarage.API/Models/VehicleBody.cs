using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;
/// <summary>
/// Represents a vehicle body type in the LuxGarage system, containing properties for the vehicle body's ID, name, 
/// and a collection of vehicles associated with the body type. This class serves as a data model for vehicle body types in the application, 
/// allowing for the storage and retrieval of vehicle body type information, including the details of the vehicle body type and 
/// its associated vehicles. This class also includes a collection of vehicles associated with the body type, 
/// enabling the application to manage and display the vehicle body type's vehicles effectively. 
/// </summary>
public class VehicleBody
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
