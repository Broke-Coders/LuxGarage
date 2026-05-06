namespace LuxGarage.API.DTOs.Requests
{
    public class UpdateEmployeeRequest
    {
        public required string Login { get; set; }
        public int WorkplaceId { get; set; }
        public int PermissionId { get; set; }
    }
}
