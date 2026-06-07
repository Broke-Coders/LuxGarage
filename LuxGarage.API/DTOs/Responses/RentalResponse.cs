namespace LuxGarage.API.DTOs.Responses;

public class RentalResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public required string VehicleName { get; set; }
    public required string CustomerEmail { get; set; }
}