using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Data; 
using Xunit;

namespace LuxGarage.Tests;

/// <summary>
/// Shared database fixture for integration tests. 
/// </summary>
/// <remarks>
/// This fixture manages a PostgreSQL container that is shared across multiple test classes.
/// It ensures that the database is set up and torn down properly, allowing tests to run against a consistent database environment.
/// </remarks>
/// 
public class SharedDatabaseFixture : IAsyncLifetime
{
    /// <summary>
    /// The PostgreSQL container instance that will be used for the tests. It is initialized in the constructor and started in the InitializeAsync method.
    /// </summary>
    public PostgreSqlContainer DbContainer { get; private set; }

    /// <summary>
    ///  Constructor that initializes the PostgreSQL container with the specified configuration. 
    /// </summary>
    /// <remarks>
    /// The container is not started here, it will be started in the InitializeAsync method to ensure that it is ready for use when the tests run.
    /// </remarks>
    public SharedDatabaseFixture()
    {
        // configure and create the PostgreSQL container
        DbContainer = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("testDb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    }

    /// <summary>
    /// Asynchronous initialization method that starts the PostgreSQL container and applies any pending migrations to ensure the database is ready for testing.
    /// </summary>
    /// <remarks>
    /// This method is called before any tests are run, allowing the tests to interact with a fully initialized database.
    /// It uses a temporary RentalContext to apply migrations, ensuring that the database schema is up to date before tests execute.
    /// </remarks>
    /// 
    /// <returns>
    /// A Task that represents the asynchronous operation of initializing the database fixture, including starting the container and applying migrations.
    /// </returns>
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

    /// <summary>
    /// Asynchronous disposal method that stops and disposes of the PostgreSQL container after all tests have completed.
    /// </summary>
    /// <returns>
    /// A Task that represents the asynchronous operation of disposing the database fixture, including stopping and disposing the container.
    /// </returns>
    public async Task DisposeAsync()
    {
        // stop and dispose container
        await DbContainer.DisposeAsync();
    }
}

