using System;

namespace LuxGarage.API.DTOs.VehicleImage.Responses;

public class VehicleImageResponse
{
    public int Id { get; set; }
    public int vehicleId { get; set; }
    public string StorageKey { get; set; } = default!;
    public string OriginalFileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long FileSize { get; set; }
    public int SortOrder { get; set; }
}
