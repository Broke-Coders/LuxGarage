namespace LuxGarage.API.Features.Users;

public class EmployeeResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; } 
    public int WorkplaceId { get; set; }
    public string? WorkplaceName { get; set; } 
}

public class UpdateEmployeeRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public UserRole Role { get; set; }
    public int WorkplaceId { get; set; }
    public bool IsActive { get; set; } 
}

public class ChangeEmployeeStatusRequest
{
    public EmployeeStatus Status { get; set; }
}