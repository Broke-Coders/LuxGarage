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
}