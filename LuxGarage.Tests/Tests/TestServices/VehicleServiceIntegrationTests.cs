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

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCorrectly()
    {
        var addedVehicle = new VehicleBuilder().WithBrand("TEST BRAND")
                                               .WithStatus(VehicleStatus.Retired)
                                               .Build();

        context.Vehicles.Add(addedVehicle);
        await context.SaveChangesAsync();

        var updateRequest = new UpdateVehicleRequest
        {
            Mileage = 1000,
            Status = VehicleStatus.Available
        };
        
        var result = await _service.UpdateAsync(addedVehicle.Id, updateRequest);

        result.Should().NotBeNull();
        context.Vehicles.Any(v => v.Status == VehicleStatus.Available && v.Mileage == 1000).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException()
    {
        
        var updateRequest = new UpdateVehicleRequest
        {
            Mileage = 1000,
            Status = VehicleStatus.Available
        };
        
        int wrongId = 9999;

        Func<Task> act = async () => await _service.UpdateAsync(wrongId, updateRequest);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Vehicle with ID {wrongId} does not exist.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAndReturnTrue()
    {
        var v1 = new VehicleBuilder()
                    .WithBrand("TEST BRAND")
                    .WithLicensePlate("TST")
                    .Build();

        context.Vehicles.Add(v1);
        await context.SaveChangesAsync();

        var result = await _service.DeleteAsync(v1.Id);

        result.Should().BeTrue();
        context.Vehicles.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotDeleteAndReturnFalse()
    {
        var v1 = new VehicleBuilder()
                    .WithBrand("TEST BRAND")
                    .WithLicensePlate("TST")
                    .Build();

        context.Vehicles.Add(v1);
        await context.SaveChangesAsync();

        var result = await _service.DeleteAsync(v1.Id+9999);


        result.Should().BeFalse();
        context.Vehicles.Should().NotBeNullOrEmpty();
    }

}