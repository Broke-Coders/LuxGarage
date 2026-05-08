using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Data; 
using Xunit;

namespace LuxGarage.Tests;

public class SharedDatabaseFixture : IAsyncLifetime
{
    public PostgreSqlContainer DbContainer { get; private set; }

    public SharedDatabaseFixture()
    {
        // configure and create the PostgreSQL container
        DbContainer = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("testDb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    }

    public async Task InitializeAsync()
    {
        // start container
        await DbContainer.StartAsync();

        // get connection string
        var options = new DbContextOptionsBuilder<RentalContext>()
            .UseNpgsql(DbContainer.GetConnectionString()) 
            .Options;

        // create temporary context to run migrations
        await using var context = new RentalContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        // stop and dispose container
        await DbContainer.DisposeAsync();
    }
}

