namespace LuxGarage.API.DTOs.Requests
{
    /// <summary>
    /// DTO for changing a workplace's details, containing the new country, city, street, and building number to be set for the workplace.
    /// </summary>
    public class ChangeWorkplaceRequest
    {
        public required string Country { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required int BuildingNumber { get; set; }

    }
}
