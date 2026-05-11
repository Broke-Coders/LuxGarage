namespace LuxGarage.API.Models;

/// <summary>
/// Represents a permission level in the LuxGarage system, containing properties for the permission's ID, 
/// name, and a collection of employees associated with the permission.
/// This class serves as a data model for permission levels in the application, 
/// allowing for the storage and retrieval of permission information and their associations with employees in the system.
/// </summary>
public class Permission
{
    public int Id { get; set; }

    public string? Name {get; set;}

    public ICollection<Employee> Employees {get;} = new List<Employee>();
}
