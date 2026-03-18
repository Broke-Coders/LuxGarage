using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class VehicleColor
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string HtmlColor { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
