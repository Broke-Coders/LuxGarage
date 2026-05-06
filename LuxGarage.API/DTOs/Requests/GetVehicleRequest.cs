namespace LuxGarage.API.DTOs.Requests.Vehicle;

public class GetVehiclesRequest
{
    public string? SortBy { get; set; } = "id";
    public bool Descending { get; set; }
}