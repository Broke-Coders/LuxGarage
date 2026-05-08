namespace LuxGarage.API.DTOs.Requests;
/// <summary>
/// DTO for user registration, containing the user's login, password, workplace ID, 
/// and permission ID to be used for creating a new user account in the system.
/// </summary>
public class RegisterRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public int WorkplaceId { get; set; }
    public int PermissionId { get; set; }
}