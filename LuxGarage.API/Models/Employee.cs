using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;

/// <summary>
/// Represents an employee in the LuxGarage system, containing properties for the employee's ID, login credentials, workplace association, 
/// and permission level. This class serves as a data model for employees in the application, allowing for the storage and retrieval of employee 
/// information, their assigned workplace, and their permissions within the system. 
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public required string Login { get; set;}
    public required string Password { get; set; }

    public int WorkplaceId { get; set; }
    public Workplace Workplace { get; set; } = null!;

    public int PermissionId { get; set; }

    public Permission Permission {get; set;} = null!;
}
