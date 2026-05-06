using LuxGarage.API.Models;

namespace LuxGarage.API.DTOs.Responses
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public int WorkplaceId { get; set; }
        public string WorkplaceCity { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
