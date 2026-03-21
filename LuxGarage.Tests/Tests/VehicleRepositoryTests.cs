using FluentAssertions;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.Tests;

public class VehicleRepositoryTests : VehicleRepositoryTestBase 
{
    private VehicleRepository createRepository() 
        => new VehicleRepository(context);
        
    [Fact]
    public async Task AddAsync_ShouldPersistVehicle()
    {
        var repo = createRepository();
        var vehicle = new Vehicle
        {
            Id = 1,
            VehicleBrandId = SeedBrandId,
            Horsepower = 122,
            LicensePlate = "K1DIS",
            Mileage = 124000,
            VehicleBodyId = SeedBodyId,
            VehicleColorId = SeedColorId
        };
        await repo.AddAsync(vehicle);
        await context.SaveChangesAsync();

        vehicle.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectVehicle()
    {
        var repo = createRepository();
        var vehicle = new Vehicle
        {
            Id = 1,
            VehicleBrandId = SeedBrandId,
            Horsepower = 122,
            LicensePlate = "K1DIS",
            Mileage = 124000,
            VehicleBodyId = SeedBodyId,
            VehicleColorId = SeedColorId
        };

        await repo.AddAsync(vehicle);
        await context.SaveChangesAsync();

        var found = await repo.GetByIdAsync(vehicle.Id);

        found.Should().NotBeNull();
        found!.LicensePlate.Should().Be("K1DIS");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoExists()
    {
        var repo = createRepository();
        var result = await repo.GetByIdAsync(9999);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCorrectlyDelete()
    {
        var repo = createRepository();
        var vehicle = new Vehicle
        {
            Id = 1,
            VehicleBrandId = SeedBrandId,
            Horsepower = 122,
            LicensePlate = "K1DIS",
            Mileage = 124000,
            VehicleBodyId = SeedBodyId,
            VehicleColorId = SeedColorId
        };
    
        await repo.AddAsync(vehicle);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(vehicle.Id);
        await context.SaveChangesAsync();

        var deleted = await context.Vehicles.FindAsync(vehicle.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCorrectlyUpdate()
    {
        var repo = createRepository();
        var vehicle = new Vehicle
        {
            Id = 1,
            VehicleBrandId = SeedBrandId,
            Horsepower = 122,
            LicensePlate = "K1DIS",
            Mileage = 124000,
            VehicleBodyId = SeedBodyId,
            VehicleColorId = SeedColorId
        };

        await repo.AddAsync(vehicle);
        await context.SaveChangesAsync();

        vehicle.Mileage = 200000;
        await repo.UpdateAsync(vehicle, vehicle.Id);
        await context.SaveChangesAsync();
        
        var updated = await context.Vehicles.FindAsync(vehicle.Id);
        Assert.Equal(200000, updated!.Mileage);
    }
}