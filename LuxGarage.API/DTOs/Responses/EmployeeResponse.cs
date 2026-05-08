using LuxGarage.API.Models;


namespace LuxGarage.API.DTOs.Responses
{
    /// <summary>
    /// DTO for employee response, containing the employee's ID, login, workplace ID and city, 
    /// permission ID and name to be returned in API responses when retrieving employee information.
    /// </summary>
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
