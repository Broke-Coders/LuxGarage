using LuxGarage.API.Models;
using LuxGarage.API.Features.Vehicles;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Vehicle instances.
/// </summary>
public class VehicleBuilder
{
    private string _brand = "BMW";
    private string _model = "M4";
    private string _licensePlate = "WA 12345";
    private decimal _horsepower = 510;
    private int _mileage = 15000;
    private int _year = 2022;
    private float _speedToHundred = 3.9f;
    private string _engineName = "V8 Turbo";
    private EngineType _engineType = EngineType.Gasoline;
    private VehicleBodyType _bodyType = VehicleBodyType.Coupe;
    private VehicleColor _color = VehicleColor.Blue;
    private VehicleStatus _status = VehicleStatus.Available;

    public VehicleBuilder WithBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    public VehicleBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleBuilder WithLicensePlate(string licensePlate)
    {
        _licensePlate = licensePlate;
        return this;
    }

    public VehicleBuilder WithHorsepower(decimal horsepower)
    {
        _horsepower = horsepower;
        return this;
    }

    public VehicleBuilder WithMileage(int mileage)
    {
        _mileage = mileage;
        return this;
    }

    public VehicleBuilder WithYear(int year)
    {
        _year = year;
        return this;
    }

    public VehicleBuilder WithSpeedToHundred(float speedToHundred)
    {
        _speedToHundred = speedToHundred;
        return this;
    }

    public VehicleBuilder WithEngineName(string engineName)
    {
        _engineName = engineName;
        return this;
    }

    public VehicleBuilder WithEngineType(EngineType engineType)
    {
        _engineType = engineType;
        return this;
    }

    public VehicleBuilder WithBodyType(VehicleBodyType bodyType)
    {
        _bodyType = bodyType;
        return this;
    }

    public VehicleBuilder WithColor(VehicleColor color)
    {
        _color = color;
        return this;
    }

    public VehicleBuilder WithStatus(VehicleStatus status)
    {
        _status = status;
        return this;
    }

    public Vehicle Build() => new Vehicle
    {
        Brand = _brand,
        Model = _model,
        LicensePlate = _licensePlate,
        Horsepower = _horsepower,
        Mileage = _mileage,
        Year = _year,
        SpeedToHundred = _speedToHundred,
        EngineName = _engineName,
        EngineType = _engineType,
        BodyType = _bodyType,
        Color = _color,
        Status = _status
    };
}
