using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.DTOs.VehicleImage.Requests;

public class CreateManyImagesRequest
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public List<IFormFile> Images { get; set; } = new();

    public int? PrimaryImageIndex { get; set; }
}