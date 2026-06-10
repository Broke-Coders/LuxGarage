using AutoMapper;
using Azure.Core;
using FluentAssertions;
using LuxGarage.API.Features.Vehicles;
using LuxGarage.API.Models;
using LuxGarage.Tests.Bases;
using LuxGarage.Tests.Builders;
using LuxGarage.Tests.DbContext;
using Moq;
using Xunit;

namespace LuxGarage.Tests.TestServices;
public class VehicleServiceIntegrationTests : ServiceTestBase
{
    public VehicleServiceIntegrationTests(SharedDatabaseFixture fixture) : base(fixture) {}

    private VehicleService _service => new VehicleService(context, mapper);

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllVehicles()
    {
        var v1 = new VehicleBuilder()
                        .WithBrand("TEST BRAND")
                        .WithLicensePlate("TST")
                        .Build();

        var v2 = new VehicleBuilder().Build();

        context.Vehicles.AddRange(v1, v2);
        await context.SaveChangesAsync();

        var result = await _service.GetAllAsync(new GetVehiclesRequest());

        result.Should().HaveCount(2);
        result.Should().Contain(r => r.LicensePlate == "TST");
    }


    [Fact]
    public async Task GetById_ShouldReturnCorrectVehicle()
    {
        var v1 = new VehicleBuilder()
                        .WithBrand("TEST BRAND")
                        .WithLicensePlate("TST")
                        .Build();

        var v2 = new VehicleBuilder().Build();

        context.Vehicles.AddRange(v1, v2);
        await context.SaveChangesAsync();

        var expected = mapper.Map<VehicleResponse>(v1);

        var result = await _service.GetByIdAsync(v1.Id);
    
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task GetById_ShouldReturnNull()
    {
        var v1 = new VehicleBuilder()
                        .WithBrand("TEST BRAND")
                        .WithLicensePlate("TST")
                        .Build();

        var v2 = new VehicleBuilder().Build();

        context.Vehicles.AddRange(v1, v2);
        await context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(9999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSuccessfully()
    {
        var createVehicle = new CreateVehicleRequest
        {
            Brand = "BMW",
            Model = "TEST",
            LicensePlate = "TST",
            Year = 2025,
            Horsepower = 100,
            Mileage = 1000,
            EngineType = EngineType.Gasoline,
            BodyType = VehicleBodyType.Sedan,
            Color = VehicleColor.Black,
            Status = VehicleStatus.Available
        };

        var result = await _service.CreateAsync(createVehicle);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createVehicle, options => options.ExcludingMissingMembers()
                                                                        .Excluding(v => v.Status));
        result.Status.Should().Be(createVehicle.Status.ToString());

        context.Vehicles.Any(v => v.LicensePlate == createVehicle.LicensePlate).Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException()
    {
        var createVehicle = new CreateVehicleRequest
        {
            Brand = "BMW",
            Model = "TEST",
            LicensePlate = "TST",
            Year = 2025,
            Horsepower = 100,
            Mileage = 1000,
            EngineType = EngineType.Gasoline,
            BodyType = VehicleBodyType.Sedan,
            Color = VehicleColor.Black,
            Status = VehicleStatus.Available
        };

        await _service.CreateAsync(createVehicle);

        var createVehicle2 = new CreateVehicleRequest
        {
            Brand = "BMW2",
            Model = "TEST2",
            LicensePlate = "TST",
            Year = 2022,
            Horsepower = 110,
            Mileage = 2000,
            EngineType = EngineType.Diesel,
            BodyType = VehicleBodyType.Cabriolet,
            Color = VehicleColor.White,
            Status = VehicleStatus.Rented
        };
        
        Func<Task> act = async () => await _service.CreateAsync(createVehicle2);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Vehicle with this license plate already exists.");
    }


}