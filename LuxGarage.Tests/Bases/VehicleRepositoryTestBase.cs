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


    }



    public async Task DisposeAsync()
    {
        // clean everything up by rolling back the transaction and disposing the context
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await context.DisposeAsync();
    }
}
