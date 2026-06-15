using LuxGarage.API.Models;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Offer instances.
/// </summary>
public class OfferBuilder
{
    private int _vehicleId = 1;
    private string _title = "Special Summer Deal";
    private string? _description = "A great deal for the summer.";
    private decimal _pricePerDay = 100.0m;
    private DateTime _publicationDate = DateTime.UtcNow;
    private bool _isActive = true;

    public OfferBuilder WithVehicleId(int vehicleId)
    {
        _vehicleId = vehicleId;
        return this;
    }

    public OfferBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public OfferBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public OfferBuilder WithPricePerDay(decimal pricePerDay)
    {
        _pricePerDay = pricePerDay;
        return this;
    }

    public OfferBuilder WithPublicationDate(DateTime publicationDate)
    {
        _publicationDate = publicationDate;
        return this;
    }

    public OfferBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public Offer Build() => new Offer
    {
        VehicleId = _vehicleId,
        Title = _title,
        Description = _description,
        PricePerDay = _pricePerDay,
        PublicationDate = _publicationDate,
        IsActive = _isActive
    };
}
