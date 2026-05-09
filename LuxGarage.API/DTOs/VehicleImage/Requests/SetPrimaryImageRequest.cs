using System;
using System.ComponentModel.DataAnnotations;

namespace LuxGarage.API.DTOs.VehicleImage.Requests;

public class SetPrimaryImageRequest
{
    [Required]
    public int ImageId { get; set; }
}
