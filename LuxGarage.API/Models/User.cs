using LuxGarage.API.Features.Users;

namespace LuxGarage.API.Models;

public class User 
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHas { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
}