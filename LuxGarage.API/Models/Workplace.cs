namespace LuxGarage.API.Models;

/// <summary>
/// Represents a workplace in the system, containing properties for the workplace's ID, country, city, street, building number, 
/// and a collection of employees associated with the workplace. This class serves as a data model for workplaces in the application, 
/// allowing for the storage and retrieval of workplace information, including the details of the workplace and
/// its associated employees. This class also includes a collection of employees associated with the workplace, 
/// enabling the application to manage and display the workplace's employees effectively.
/// </summary>
public class Workplace
{
    public int Id { get; set; }

    public required string Country { get; set; }

    public required string City { get; set; }

    public required string Street { get; set; }

    public required int BuildingNumber { get; set; }
    
    public ICollection<Employee> Employees { get; } = new List<Employee>();
}
