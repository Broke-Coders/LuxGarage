namespace LuxGarage.API.DTOs.Requests;

public class CreateRentalRequest
{
    public int VehicleId { get; set; }
    public required string CustomerEmail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
}