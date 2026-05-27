namespace LuxGarage.API.DTOs.Requests.Offer;

public class GetOffersRequest
{
    public string? SortBy { get; set; } = "id";
    public bool Descending { get; set; }
    public bool? OnlyAvailable { get; set; }
}