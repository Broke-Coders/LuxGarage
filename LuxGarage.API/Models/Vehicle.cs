using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxGarage.API.Models;
public class Vehicle
{
    public int Id { get; set; }

    public int VehicleBrandId { get; set; } 
    public VehicleBrand VehicleBrand{ get; set; } = null!;

    public int VehicleModelId { get; set; }
    public VehicleModel VehicleModel { get; set; } = null!;

    public int VehicleImageId { get; set; }

    public decimal Horsepower { get; set; }
    public string LicensePlate { get; set; } = null!;
    public int Mileage { get; set; }
    public int year { get; set; }
    
    public int VehicleBodyId { get; set; }
    public VehicleBody VehicleBody { get; set; } = null!;

    public int VehicleColorId { get; set; }
    public VehicleColor VehicleColor { get; set; } = null!;

    public List<Rental> Rentals {get;} = new List<Rental>();
    public List<VehicleImage> Images { get; set; } = new List<VehicleImage>();
}
