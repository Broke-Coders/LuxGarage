using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class VehiclePrice
{
    public int Id { get; set; }

    public int VehicleModelId { get; set; }
    public VehicleModel VehicleModel { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
    public decimal PricePerDay { get; set; }
}