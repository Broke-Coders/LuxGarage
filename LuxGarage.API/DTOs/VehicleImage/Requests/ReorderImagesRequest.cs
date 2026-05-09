using System;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.DTOs.VehicleImage.Requests;

public class ReorderImagesRequest
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public List<int> OrderedIds { get; set; } = new();
}
