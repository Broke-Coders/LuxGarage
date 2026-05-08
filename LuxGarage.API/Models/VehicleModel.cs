namespace LuxGarage.API.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int VehicleBrandId { get; set; }
        public VehicleBrand VehicleBrand { get; set; } = null!;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
