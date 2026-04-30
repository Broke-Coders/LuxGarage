using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.Models;

public class Workplace
{
    public int Id { get; set; }

    public required string Country { get; set; }

    public required string City { get; set; }

    public required string Street { get; set; }

    public required int BuildingNumber { get; set; }
    
    public ICollection<Employee> Employees { get; } = new List<Employee>();
}
