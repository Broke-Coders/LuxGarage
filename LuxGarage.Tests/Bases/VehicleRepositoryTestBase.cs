using System;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Testcontainers.PostgreSql;

namespace LuxGarage.Tests;

[Collection("SharedDB")]
public class VehicleRepositoryTestBase : IAsyncLifetime
{
    
    private readonly SharedDatabaseFixture _fixture;

    protected RentalContext context { get; private set; } = null!;

    private IDbContextTransaction _transaction = null!;
    protected int SeedBodyId { get; private set; }
    protected int SeedBrandId { get; private set; }
    protected int SeedColorId { get; private set; }

    public VehicleRepositoryTestBase(SharedDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {

        var options = new DbContextOptionsBuilder<RentalContext>()
            .UseNpgsql(_fixture.DbContainer.GetConnectionString())
            .Options;
        
        context = new RentalContext(options);

        _transaction = await context.Database.BeginTransactionAsync();

        await SeedDataBase();

    }

    private async Task SeedDataBase()
    {
        var brand = new VehicleBrand { Id = 1, Name = "Mazda"};
        var body = new VehicleBody {Id = 1, Name = "Sedan"};
        var color = new VehicleColor { Id = 1, HtmlColor = "#FFFFFF", Name = "White" };

        context.VehicleBrands.Add(brand);
        context.VehicleBodies.Add(body);
        context.VehicleColors.Add(color);

        SeedBodyId = body.Id;
        SeedBrandId = brand.Id;
        SeedColorId = color.Id;
        await context.SaveChangesAsync();

    }


    public async Task DisposeAsync()
    {
        // clean everything up by rolling back the transaction and disposing the context
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await context.DisposeAsync();
    }
}
