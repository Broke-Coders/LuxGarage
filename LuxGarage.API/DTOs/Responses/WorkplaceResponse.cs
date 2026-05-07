namespace LuxGarage.API.DTOs.Responses
{
    public class WorkplaceResponse
    {
        public int Id { get; set; }
        public required string Country { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required int BuildingNumber { get; set; }

    }
}
