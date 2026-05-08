using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;
/// <summary>
/// Represents a vehicle color in the LuxGarage system, containing properties for the vehicle color's ID, name, HTML color code,
/// and a collection of vehicles associated with the color. This class serves as a data model for vehicle colors in the application,
/// allowing for the storage and retrieval of vehicle color information, including the details of the vehicle color
/// and its associated vehicles. This class also includes a collection of vehicles associated with the color,
/// enabling the application to manage and display the vehicle color's vehicles effectively.
/// </summary>
public class VehicleColor
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string HtmlColor { get; set; }

    public ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
