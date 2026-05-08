namespace LuxGarage.API.DTOs.Responses
{
    /// <summary>
    /// DTO for workplace response, containing the workplace's ID, country, city, street, 
    /// and building number to be returned in API responses when retrieving workplace information.
    /// </summary>
    public class WorkplaceResponse
    {
        public int Id { get; set; }
        public required string Country { get; set; }

        public required string City { get; set; }

        public required string Street { get; set; }

        public required int BuildingNumber { get; set; }

    }
}
