namespace LuxGarage.API.Models;

/// <summary>
/// Represents a vehicle image in the LuxGarage system, containing properties for the image's ID, associated vehicle, storage key, 
/// original file name, content type, file size, sort order, and whether it is the primary image. 
/// This class serves as a data model for vehicle images in the application, allowing for the storage and retrieval of vehicle image information, 
/// including the details of the image and its association with a specific vehicle.
/// </summary>
public class VehicleImage
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string StorageKey { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long FileSize { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}