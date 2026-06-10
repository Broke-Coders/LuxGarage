namespace LuxGarage.API.Features.Rentals;

public class CreateRentalRequest
{
    public int VehicleId { get; set; }
    
    public required string CustomerEmail { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public List<int>? SelectedInsuranceIds { get; set; } 
}

public class RentalResponse
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime AppointedReturnTime { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}