using FluentAssertions;
using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.Tests.Builders;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.Tests;
/// <summary>
///  Tests for the VehicleRepository class, which handles CRUD operations for Vehicle entities in the database.
/// </summary>
/// <remarks>
/// These tests cover the basic functionality of adding, retrieving, updating, and deleting Vehicle records using
/// the VehicleRepository. Each test ensures that the repository methods interact correctly with the database context and that the expected results are returned.
/// The tests use a shared database fixture to manage the database connection and ensure isolation between tests by rolling back transactions after each test.
/// Example tests include:
/// - AddAsync_ShouldPersistVehicle: Verifies that a new Vehicle can be added and persisted
/// - GetById_ShouldReturnCorrectVehicle: Checks that a Vehicle can be retrieved by its ID and that the correct data is returned
/// - GetByIdAsync_ShouldReturnNull_WhenNoExists: Ensures that attempting to retrieve a non-existent Vehicle returns null
/// - DeleteAsync_ShouldCorrectlyDelete: Confirms that a Vehicle can be deleted and is no longer present in the database
/// - UpdateAsync_ShouldCorrectlyUpdate: Validates that a Vehicle's properties can be updated and that the changes are saved to the database
/// </remarks>
public class VehicleRepositoryTests : VehicleRepositoryTestBase 
{
    /// <summary>
    ///  Constructor for the VehicleRepositoryTests class, which initializes the test class with a shared database fixture. 
    /// </summary>
    /// <remarks>
    /// The fixture provides a shared database context for all tests in this class, 
    /// allowing for efficient setup and teardown of the database state between tests. The constructor calls the base class constructor
    /// to ensure that the context is properly initialized and available for use in the test methods.
    /// </remarks>
    /// <param name="fixture">The shared database fixture to use for the tests.</param>
    public VehicleRepositoryTests(SharedDatabaseFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// Helper method to create a new instance of the VehicleRepository using the current database context. 
    /// This method is used in each test to ensure that a fresh repository instance is available for testing.
    /// </summary>
    /// <returns>The created VehicleRepository instance.</returns>
    private VehicleRepository createRepository() 
        => new VehicleRepository(context);
        

    /// <summary>
    /// Tests that a new Vehicle can be added to the repository and persisted in the database. 
    /// </summary>
    /// <remarks>
    /// This test uses the VehicleBuilder to create a new Vehicle instance with specified properties, 
    /// adds it to the repository, and saves the changes to the database. 
    /// Finally, it asserts that the Vehicle has been assigned an ID, indicating that it was successfully persisted.
    /// </remarks>
    [Fact]
    public async Task AddAsync_ShouldPersistVehicle()
    {
        var repo = createRepository();

        var vehicle = await new VehicleBuilder()
            .WithLicensePlate("K1DIS")
            .WithMileage(124000)
            .WithHorsepower(122)
            .WithBrandAndModel("Skoda", "Octavia")
            .BuildUnsavedAsync(context);

        await repo.AddAsync(vehicle);
        await context.SaveChangesAsync();

        vehicle.Id.Should().NotBe(0);
    }

    /// <summary>
    /// Tests that a Vehicle can be retrieved by its ID and that the correct data is returned
    /// </summary>
    /// <remarks>
    /// This test first creates and saves a new Vehicle using the VehicleBuilder, then retrieves it from the repository using its ID.
    /// Finally, it asserts that the retrieved Vehicle is not null and that its LicensePlate property
    /// matches the expected value, confirming that the correct Vehicle was returned from the repository.
    /// </remarks>
    [Fact]
    public async Task GetById_ShouldReturnCorrectVehicle()
    {
        var repo = createRepository();
        var vehicle = await new VehicleBuilder()
            .WithLicensePlate("K1DIS")
            .WithMileage(124000)
            .WithHorsepower(122)
            .WithBrandAndModel("Skoda", "Octavia")
            .BuildAndSaveAsync(context);


        var found = await repo.GetByIdAsync(vehicle.Id);

        found.Should().NotBeNull();
        found!.LicensePlate.Should().Be("K1DIS");
    }

    /// <summary>
    /// Tests that attempting to retrieve a non-existent Vehicle by ID returns null.
    /// </summary>
    /// <remarks>
    /// This test calls the GetByIdAsync method of the repository with an ID that does
    /// not correspond to any existing Vehicle in the database. It then asserts that the result is null, 
    /// confirming that the repository correctly handles cases where a requested entity does not exist.
    /// </remarks>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoExists()
    {
        var repo = createRepository();
        var result = await repo.GetByIdAsync(9999);
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that a Vehicle can be deleted from the repository and that it is no longer present in the database after deletion.
    /// </summary>
    /// <remarks>
    /// This test first creates and saves a new Vehicle using the VehicleBuilder, then deletes it
    /// using the repository's DeleteAsync method. After saving the changes to the database, 
    /// it attempts to retrieve the deleted Vehicle by its ID and asserts that the result is null, 
    /// confirming that the Vehicle was successfully deleted from the database.
    /// </remarks>
    [Fact]
    public async Task DeleteAsync_ShouldCorrectlyDelete()
    {
        var repo = createRepository();
        
        var vehicle = await new VehicleBuilder()
            .WithLicensePlate("K1DIS")
            .WithMileage(124000)
            .WithHorsepower(122)
            .WithBrandAndModel("Skoda", "Octavia")
            .BuildUnsavedAsync(context);
    

        await repo.DeleteAsync(vehicle.Id);
        await context.SaveChangesAsync();

        var deleted = await context.Vehicles.FindAsync(vehicle.Id);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Tests that a Vehicle's properties can be updated using the repository and that the changes are saved to the database correctly.
    /// </summary>
    /// <remarks>
    /// This test first creates and saves a new Vehicle using the VehicleBuilder, 
    /// then updates its Mileage property and calls the repository's UpdateAsync method.
    /// After saving the changes to the database, 
    /// it retrieves the updated Vehicle by its ID and asserts that the Mileage property has been updated to the new value,
    /// confirming that the update operation was successful and that the changes were persisted in the database.
    /// </remarks>
    [Fact]
    public async Task UpdateAsync_ShouldCorrectlyUpdate()
    {
        var repo = createRepository();
        var vehicle = await new VehicleBuilder()
            .WithLicensePlate("K1DIS")
            .WithMileage(124000)
            .WithHorsepower(122)
            .WithBrandAndModel("Skoda", "Octavia")
            .BuildAndSaveAsync(context);

        vehicle.Mileage = 200000;
        await repo.UpdateAsync(vehicle, vehicle.Id);
        await context.SaveChangesAsync();
        
        var updated = await context.Vehicles.FindAsync(vehicle.Id);
        updated.Should().NotBeNull();
        updated!.Mileage.Should().Be(200000);
    }
}