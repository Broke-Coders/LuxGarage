namespace LuxGarage.API.DTOs.Requests;

public class RegisterRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public int WorkplaceId { get; set; }
    public int PermissionId { get; set; }
}