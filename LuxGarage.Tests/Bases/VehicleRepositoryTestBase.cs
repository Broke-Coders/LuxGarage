using System;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace LuxGarage.Tests;

public class VehicleRepositoryTestBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("testDb")
        .WithUsername("testuser")
        .WithPassword("testpassword")
        .Build();
    
    protected RentalContext context { get; private set; } = null!;

    protected int SeedBodyId { get; private set; }
    protected int SeedBrandId { get; private set; }
    protected int SeedColorId { get; private set; }

    public async Task InitializeAsync()
    {
        await postgres.StartAsync();

        var options = new DbContextOptionsBuilder<RentalContext>()
            .UseNpgsql(postgres.GetConnectionString())
            .Options;
        
        context = new RentalContext(options);
        await context.Database.MigrateAsync();

        var brand = new VehicleBrand { Id = 1, Name = "Mazda"};
        var body = new VehicleBody {Id = 1, Name = "Sedan"};
        var color = new VehicleColor { Id = 1, HtmlColor = "#FFFFFF", Name = "White" };

        context.VehicleBrands.Add(brand);
        context.VehicleBodies.Add(body);
        context.VehicleColors.Add(color);

        SeedBodyId = body.Id;
        SeedBrandId = brand.Id;
        SeedColorId = color.Id;
    }

    public async Task DisposeAsync()
    {
        await context.DisposeAsync();
        await postgres.DisposeAsync();
    }
}
