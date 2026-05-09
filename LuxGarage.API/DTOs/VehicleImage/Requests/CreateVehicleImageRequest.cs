using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.DTOs.VehicleImage.Requests;

public class CreateVehicleImageRequest
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public IFormFile Image { get; set; } = default!;

    public bool IsPrimary { get; set; }
}
