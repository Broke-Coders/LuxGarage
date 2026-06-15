namespace LuxGarage.API.Features.Vehicles;

public class GetVehiclesRequest
{
    public string? SearchTerm { get; set; }
    public VehicleBodyType? BodyType { get; set; }
    public VehicleStatus? Status {get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }

    public string? SortBy { get; set; }
    public bool Descending { get; set; }
}

public class CreateVehicleRequest
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string EngineName { get; set; } = null!;
    public int Year { get; set; }
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }
    public float ToHundred { get; set; }
    
    public EngineType EngineType { get; set; }
    public VehicleBodyType BodyType { get; set; }
    public VehicleColor Color { get; set; }
    public VehicleStatus Status { get; set; }
    
    public List<IFormFile>? Images { get; set; } 
}

public class UpdateVehicleRequest
{
    public int Mileage { get; set; }
    public VehicleStatus Status { get; set; }
}

public class VehicleResponse
{
    public int Id { get; set; }
    public string Brand { get; set; } = null!;
    public required string LicensePlate { get; set; }
    public int Year { get; set; }
    public decimal Horsepower { get; set; }
    public int Mileage { get; set; }
    public required string Status { get; set; } 
}