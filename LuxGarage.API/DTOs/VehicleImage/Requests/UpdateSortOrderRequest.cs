using System;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.DTOs.VehicleImage.Requests;

public class UpdateSortOrderRequest
{
    [Required]
    public int ImageId { get; set; }

    [Range(0, int.MaxValue)]
    public int NewSortOrder { get; set; }
}