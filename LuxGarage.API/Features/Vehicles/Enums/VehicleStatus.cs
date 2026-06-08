namespace LuxGarage.API.Features.Vehicles;

/// <summary>
/// Indicates the current operational availability of the vehicle.
/// </summary>
public enum VehicleStatus
{
    /// <summary>Vehicle is ready and available for rental.</summary>
    Available = 1,
    
    /// <summary>Vehicle is currently rented by a customer.</summary>
    Rented = 2,
    
    /// <summary>Vehicle is undergoing routine maintenance or repairs.</summary>
    Maintenance = 3,
    
    /// <summary>Vehicle is out of service (e.g., damaged, waiting for parts).</summary>
    OutOfService = 4,
    
    /// <summary>Vehicle has been retired from the fleet.</summary>
    Retired = 5
}