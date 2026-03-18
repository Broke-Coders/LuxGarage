using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Vehicle
{
    [Key]
    public int Id { get; set; }

    public int BrandId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Horsepower { get; set; }

    public int Mileage { get; set; }

    public int BodyId { get; set; }

    public int ColorId { get; set; }

    [ForeignKey(nameof(BrandId))]
    public VehicleBrand Brand { get; set; }

    [ForeignKey(nameof(BodyId))]
    public VehicleBody Body { get; set; }

    [ForeignKey(nameof(ColorId))]
    public VehicleColor Color { get; set; }
}
