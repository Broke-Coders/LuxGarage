namespace LuxGarage.API.Models;

/// <summary>
/// Represents a vehicle status in the LuxGarage system, containing properties for the status's ID, description, starting date, 
/// and optional end date. This class serves as a data model for vehicle statuses in the application, 
/// allowing for the storage and retrieval of vehicle status information, including the details of the status and its validity period. 
/// This class enables the application to manage and display vehicle status information effectively, providing insights 
/// into the current state of a vehicle and its history of statuses over time.
/// </summary>
public class VehicleStatus
{
    public int Id { get; set; }

    public string Description { get; set; } = "UNKNOWN";

    public required DateTime StartingDate { get; set; }
    public DateTime? DateToEnd { get; set; }
}