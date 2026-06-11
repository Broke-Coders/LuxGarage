using LuxGarage.API.Features.Users;

namespace LuxGarage.API.Models;

/// <summary>
/// Represents an application user including identity, authentication,
/// and role information.
/// </summary>
public class User 
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmployeeRequested { get; set; } = false;
}

/// <summary>
/// Represents a customer in the LuxGarage system, containing comprehensive profile information including 
/// contact details, driver's license data, and rental history tracking. This class serves as the 
/// primary data model for customer management and rental associations.
/// </summary>
public class Customer : User
{
    public required string PhoneNumber { get; set; }
    public required string LicenseNumber { get; set; }
    public int BorrowCounter { get; set; } = 0;
    public ICollection<Rental> Rentals { get; } = new List<Rental>();
}

/// <summary>
/// Represents an employee in the LuxGarage system, containing properties for the employee's ID, login credentials, workplace association, 
/// and permission level. This class serves as a data model for employees in the application, allowing for the storage and retrieval of employee 
/// information, their assigned workplace, and their permissions within the system. 
/// </summary>
public class Employee : User
{
    public int WorkplaceId { get; set; }
    public Workplace Workplace { get; set; } = null!;
}