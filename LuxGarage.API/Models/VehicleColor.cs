using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;
public class VehicleColor
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string HtmlColor { get; set; }

    public ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
