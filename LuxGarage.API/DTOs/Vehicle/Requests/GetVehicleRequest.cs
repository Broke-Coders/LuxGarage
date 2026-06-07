namespace LuxGarage.API.DTOs.Requests.Vehicle;

/// <summary>
/// DTO for retrieving a list of vehicles, containing optional sorting 
/// parameters such as the field to sort by and the sort direction (ascending or descending).
/// </summary>
public class GetVehiclesRequest
{
    public string? SortBy { get; set; } = "id";
    public bool Descending { get; set; }
    public string? SearchTerm { get; set; }
    public int? BrandId { get; set; }
    public int? BodyTypeId { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }

}