namespace LuxGarage.API.DTOs.Requests
{
    /// <summary>
    /// DTO for updating an employee's details, containing the new login, workplace ID, and permission ID to be set for the employee.
    /// </summary>
    public class UpdateEmployeeRequest
    {
        public required string Login { get; set; }
        public int WorkplaceId { get; set; }
        public int PermissionId { get; set; }
    }
}
