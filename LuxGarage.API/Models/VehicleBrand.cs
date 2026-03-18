using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class VehicleBrand
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
