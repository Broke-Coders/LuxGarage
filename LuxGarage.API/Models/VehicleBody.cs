using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class VehicleBody
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    // Właściwość nawigacyjna
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
