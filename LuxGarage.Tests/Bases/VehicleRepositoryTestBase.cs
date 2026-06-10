using System;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Testcontainers.PostgreSql;
using LuxGarage.Tests.DbContext;

namespace LuxGarage.Tests.Bases;

/// <summary>
/// Base class for VehicleRepository tests, providing a shared database context and transaction management for each test.
/// </summary>
[Collection("SharedDB")]
public class VehicleRepositoryTestBase : IAsyncLifetime
{
    
    private readonly SharedDatabaseFixture _fixture;

    protected RentalContext context { get; private set; } = null!;

    private IDbContextTransaction _transaction = null!;

    /// <summary>
    /// Initializes a new instance of the VehicleRepositoryTestBase class with the provided shared database fixture.
    /// </summary>
    /// <param name="fixture">The shared database fixture.</param>
    public VehicleRepositoryTestBase(SharedDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Initializes the database context and begins a new transaction for each test. 
    /// This ensures that each test runs in isolation and can be rolled back after completion.
    /// </summary>
    public async Task InitializeAsync()
    {

        var options = new DbContextOptionsBuilder<RentalContext>()
            .UseNpgsql(_fixture.DbContainer.GetConnectionString())
            .Options;
        
        context = new RentalContext(options);

        _transaction = await context.Database.BeginTransactionAsync();


    }

    /// <summary>
    /// Rolls back the transaction and disposes of the database context after each test, 
    /// ensuring that any changes made during the test are not persisted to the database.
    /// </summary>
    public async Task DisposeAsync()
    {
        // clean everything up by rolling back the transaction and disposing the context
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await context.DisposeAsync();
    }
}
