namespace LuxGarage.API.Features.Offers;

public class GetOffersRequest
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
}

public class CreateOfferRequest
{
    public int VehicleId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal InitialPricePerDay { get; set; }
}

public class UpdateOfferRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public decimal? NewPricePerDay { get; set; } 
}

public class OfferListItemResponse
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Mileage { get; set; } = null!;
    public int Year { get; set; }
    public decimal Horsepower { get; set; }
    public string Engine { get; set; } = null!;
    public string ZeroToHundred { get; set; } = null!;
    public string BodyName { get; set; } = null!;
    public string? PrimaryImageUrl { get; set; }
    public bool IsActive { get; set; }
}

public class OfferDetailsResponse : OfferListItemResponse
{
    public string? Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public string BodyType { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Status { get; set; } = null!;
}