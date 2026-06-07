namespace LuxGarage.API.Models;

/// <summary>
/// Defines the possible states of a vehicle within the LuxGarage fleet.
/// </summary>
public enum StatusType
{
    /// <summary>The vehicle is ready and available for rental.</summary>
    Available,
    /// <summary>The vehicle is currently being used by a customer.</summary>
    Rented,
    /// <summary>The vehicle is undergoing technical maintenance or repair.</summary>
    Maintenance,
    /// <summary>The vehicle is withdrawn from service and cannot be rented.</summary>
    Unavailable
}


/// <summary>
/// Represents a vehicle status record in the LuxGarage system, tracking the state of a vehicle 
/// (e.g., Available, Rented, Maintenance) over a specific validity period. This class enables 
/// the application to manage and display the current state and historical status changes of vehicles.
/// </summary>
public class VehicleStatus
{
    /// <summary>Gets or sets the unique identifier for the vehicle status.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the type of the status, using the StatusType enum for type-safe state management.</summary>
    public StatusType Name { get; set; } = StatusType.Unavailable;

    /// <summary>Gets or sets an optional detailed description or notes regarding the status change.</summary>
    public string Description { get; set; } = "UNKNOWN";

    public bool IsAvailable { get; set; }    

    public required DateTime StartingDate { get; set; }
    public DateTime? DateToEnd { get; set; }
}