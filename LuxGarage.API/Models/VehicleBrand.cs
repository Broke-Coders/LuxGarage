using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;
public class VehicleBrand
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
