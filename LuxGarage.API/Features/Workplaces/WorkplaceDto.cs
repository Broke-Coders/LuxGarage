namespace LuxGarage.API.Features.Workplaces;

public class WorkplaceResponse
{
    public int Id { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string BuildingNumber { get; set; }
}

public class ChangeWorkplaceRequest
{
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string BuildingNumber { get; set; }
}