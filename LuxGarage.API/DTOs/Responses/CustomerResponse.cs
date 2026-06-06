namespace LuxGarage.API.DTOs.Responses;

public class CustomerResponse
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string LicenseNumber { get; set; }
    public int BorrowCounter { get; set; }
}