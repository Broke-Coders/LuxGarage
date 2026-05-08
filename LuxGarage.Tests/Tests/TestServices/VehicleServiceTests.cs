using AutoMapper;
using FluentAssertions;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Models;
using LuxGarage.API.Profiles;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Moq;
using Xunit;

namespace LuxGarage.Tests.TestServices;

/// <summary>
/// Tests for the VehicleService class, which provides business logic for managing Vehicle entities.
/// </summary>
/// <remarks>
/// These tests cover the basic functionality of the VehicleService, 
/// including retrieving a Vehicle by ID and handling cases where a Vehicle is not found.
/// The tests use Moq to create a mock implementation of the IVehicleRepository, 
/// allowing for isolation of the service logic from the data access layer.
/// Example tests include:
/// - GetByIdAsync_ShouldReturnNull_WhenVehicleNotFound: Verifies that the service returns null when a Vehicle with the specified ID does not exist.
/// - GetByIdAsync_ShouldReturnDetails_WhenVehicleExists: Checks that the service returns the correct Vehicle details when a Vehicle with the specified ID exists.
/// - CreateAsync_ShouldThrowException_WhenLicensePlateExists: Ensures that attempting to create a Vehicle with a license plate that already exists throws an InvalidOperationException.
/// - CreateAsync_ShouldReturnResponse_WhenDataIsValid: Validates that a Vehicle can be created successfully when the provided data is valid and that the correct response is returned.
/// - GetAllAsync_ShouldSortByMileageDescending_WhenRequested: Confirms that all Vehicles can be retrieved and sorted by mileage in descending order when requested.
/// </remarks>
public class VehicleServiceTests
{
    private readonly Mock<IVehicleRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly IVehicleService _service;

    /// <summary>
    /// Constructor for the VehicleServiceTests class, which initializes the test class with a mock repository and an AutoMapper instance.
    /// </summary>
    public VehicleServiceTests()
    {
        _repoMock = new Mock<IVehicleRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new VehicleService(_repoMock.Object, _mapper);
    }

    /// <summary>
    /// Tests that attempting to retrieve a Vehicle by ID that does not exist returns null.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenVehicleNotFound()
    {
        _repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((Vehicle?)null!);

        var result = await _service.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that a Vehicle can be retrieved by its ID and that the correct details are returned when the Vehicle exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDetails_WhenVehicleExists()
    {
        var vehicle = new Vehicle {Id = 1, LicensePlate = "K1DIS"};

        _repoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(vehicle);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.LicensePlate.Should().Be("K1DIS");
    }


    /// <summary>
    /// Tests that attempting to create a Vehicle with a license plate that already exists throws an InvalidOperationException.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenLicensePlateExists()
    {
        var existingVehicle = new Vehicle { Id = 1, LicensePlate = "K1DIS" };

        _repoMock.Setup(repo => repo.GetByLicensePlateAsync("K1DIS"))
                 .ReturnsAsync(existingVehicle);

        var request = new CreateVehicleRequest { LicensePlate = "K1DIS" };

        Func<Task> act = async () => await _service.CreateAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Vehicle with this license plate already exists.");
    
        _repoMock.Verify(repo => repo.AddAsync(It.IsAny<Vehicle>()), Times.Never);
    }

    /// <summary>
    /// Tests that a Vehicle can be created successfully when the provided data is valid and that the correct response is returned.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldReturnResponse_WhenDataIsValid()
    {
        var request = new CreateVehicleRequest { LicensePlate = "NEW-123" };
        _repoMock.Setup(repo => repo.GetByLicensePlateAsync("NEW-123"))
                 .ReturnsAsync((Vehicle)null!);

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result!.LicensePlate.Should().Be("NEW-123");
        _repoMock.Verify(repo => repo.AddAsync(It.IsAny<Vehicle>()), Times.Once);
    }

    /// <summary>
    /// Tests that all Vehicles can be retrieved and sorted by mileage in descending order when requested.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldSortByMileageDescending_WhenRequested()
    {
        var vehicles = new List<Vehicle>
        {
            new Vehicle { Id = 1, Mileage = 1000 },
            new Vehicle { Id = 2, Mileage = 2000 },
            new Vehicle { Id = 3, Mileage = 1500 }
        };

        _repoMock.Setup(repo => repo.GetAllAsync())
                 .ReturnsAsync(vehicles);

        var request = new GetVehiclesRequest
        {
            SortBy = "mileage",
            Descending = true
        };

        var result = await _service.GetAllAsync(request);

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.First().Mileage.Should().Be(2000);
        result.Last().Mileage.Should().Be(1000);
    }
}