using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Workplace
{
    [Key]
    public int Id { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public int BuildingNumber { get; set; }
    
    public ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
