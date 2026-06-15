using LuxGarage.API.Models;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Workplace instances.
/// </summary>
public class WorkplaceBuilder
{
    private string _country = "Poland";
    private string _city = "Warsaw";
    private string _street = "Golden Street";
    private string _buildingNumber = "44";

    public WorkplaceBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public WorkplaceBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public WorkplaceBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public WorkplaceBuilder WithBuildingNumber(string buildingNumber)
    {
        _buildingNumber = buildingNumber;
        return this;
    }

    public Workplace Build() => new Workplace
    {
        Country = _country,
        City = _city,
        Street = _street,
        BuildingNumber = _buildingNumber
    };
}
