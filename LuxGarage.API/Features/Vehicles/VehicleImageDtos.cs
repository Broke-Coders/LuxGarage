using Microsoft.Identity.Client;

public class UploadVehicleImagesRequest
{
    public int VehicleId { get; set; }
    public required List<IFormFile> Images { get; set; }
    public int? PrimaryImageIndex { get; set; }
}

public class SetPrimaryImageRequest
{
    public int  ImageId { get; set; }
}

public class ReorderImagesRequest
{
    public int VehicleId { get; set; }
    public required List<int> OrderedImageIds { get; set; }
}

public class VehicleImageResponse
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public required string StorageKey { get; set; }
    public required string OriginalFileName { get; set; }
    public long FileSize { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public required string Url { get; set; }
}