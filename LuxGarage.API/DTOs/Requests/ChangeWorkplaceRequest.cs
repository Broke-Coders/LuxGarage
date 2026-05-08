namespace LuxGarage.API.DTOs.Requests
{
    public class ChangeWorkplaceRequest
    {
        public required string Country { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required int BuildingNumber { get; set; }

    }
}
