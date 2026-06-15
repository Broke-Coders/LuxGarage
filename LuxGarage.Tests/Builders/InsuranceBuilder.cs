using LuxGarage.API.Models;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Insurance instances.
/// </summary>
public class InsuranceBuilder
{
    private string _name = "Basic Protection";
    private decimal _pricePerDay = 25.0m;
    private bool _isActive = true;

    public InsuranceBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public InsuranceBuilder WithPricePerDay(decimal pricePerDay)
    {
        _pricePerDay = pricePerDay;
        return this;
    }

    public InsuranceBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public Insurance Build() => new Insurance
    {
        Name = _name,
        PricePerDay = _pricePerDay,
        IsActive = _isActive
    };
}
