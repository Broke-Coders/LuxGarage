namespace LuxGarage.API.Features.Insurances;

public class CreateInsuranceRequest
{
    public required string Name { get; set; }
    public required decimal PricePerDay { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateInsuranceRequest
{
    public required string Name { get; set; }
    public required decimal PricePerDay { get; set; }
    public bool IsActive { get; set; }
}

public class InsuranceResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PricePerDay { get; set; }
    public bool IsActive { get; set; }
}