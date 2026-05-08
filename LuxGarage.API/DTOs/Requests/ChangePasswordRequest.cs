namespace LuxGarage.API.DTOs.Requests
{
    /// <summary>
    /// DTO for changing a user's password, containing the new password to be set for the user.
    /// </summary>
    public class ChangePasswordRequest
    {
        public required string  NewPassword { get; set; }
    }
}
