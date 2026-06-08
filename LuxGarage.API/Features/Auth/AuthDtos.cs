namespace LuxGarage.API.Features.Auth;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponse
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}

public class RegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public bool IsEmployee { get; set; } 

    // For Customers
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }

    // For Employees
    public int? WorkplaceId { get; set; }
}

public class RegisterResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}