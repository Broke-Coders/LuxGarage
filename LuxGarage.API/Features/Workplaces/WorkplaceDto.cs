namespace LuxGarage.API.Features.Branches;

public class BranchResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; } // Złączyliśmy wcześniej Country, City, Street w Address
}

public class ChangeBranchRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
}