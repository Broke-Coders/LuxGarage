using AutoMapper;
using FluentAssertions;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Services.Implementations;
using LuxGarage.API.Models;
using LuxGarage.API.Profiles;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Moq;
using Xunit;
using LuxGarage.API.DTOs.Requests;

namespace LuxGarage.Tests.TestServices;

/// <summary>
/// Unit tests for the WorkplaceService class, which is responsible for managing workplace data in the LuxGarage API.
/// </summary>
public class WorkPlaceServiceTests
{
    private readonly Mock<IWorkplaceRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly IWorkplaceService _service;

    /// <summary>
    /// Initializes a new instance of the WorkPlaceServiceTests class, 
    /// setting up the necessary dependencies for testing the WorkplaceService,
    /// </summary>
    public WorkPlaceServiceTests()
    {
        _repoMock = new Mock<IWorkplaceRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new WorkplaceService(_repoMock.Object, _mapper);
    }

    /// <summary>
    /// Tests the GetAllAsync method of the WorkplaceService, ensuring that it returns all workplaces 
    /// correctly by mocking the repository's response and verifying the results using FluentAssertions.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWorkplaces()
    {
        var workplaces = new List<Workplace>
        {
            new Workplace { Id = 1, Country = "Country 1", City = "City 1", Street = "Street 1", BuildingNumber = 1 },
            new Workplace { Id = 2, Country = "Country 2", City = "City 2", Street = "Street 2", BuildingNumber = 2 }
        };
        _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(workplaces);

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result.Should().ContainSingle(w => w.Id == 1 && w.Country == "Country 1" &&
         w.City == "City 1" && w.Street == "Street 1" && w.BuildingNumber == 1);

        result.Should().ContainSingle(w => w.Id == 2 && w.Country == "Country 2" &&
         w.City == "City 2" && w.Street == "Street 2" && w.BuildingNumber == 2);
    }

    /// <summary>
    /// Tests the GetByIdAsync method of the WorkplaceService, ensuring that it returns the
    /// correct workplace when it exists by mocking the repository's response and verifying the results using FluentAssertions.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnWorkplace_WhenWorkplaceExists()
    {
        var workplace = new Workplace { Id = 1, Country = "Country 1", City = "City 1", Street = "Street 1", BuildingNumber = 1 };
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(workplace);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Country.Should().Be("Country 1");
        result.City.Should().Be("City 1");
        result.Street.Should().Be("Street 1");
        result.BuildingNumber.Should().Be(1);

    }


    /// <summary>
    /// Tests the GetByIdAsync method of the WorkplaceService, ensuring that it returns null when the workplace does not exist.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenWorkplaceDoesNotExist()
    {
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Workplace?)null);

        var result = await _service.GetByIdAsync(1);

        result.Should().BeNull();
    }


    /// <summary>
    /// Tests the CreateAsync method of the WorkplaceService, 
    /// ensuring that it creates a new workplace correctly by mocking the repository's AddAsync method.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldCreateWorkplace()
    {
        var request = new ChangeWorkplaceRequest
        {
            Country = "Country 1",
            City = "City 1",
            Street = "Street 1",
            BuildingNumber = 1
        };

        var createdWorkplace = new Workplace
        {
            Id = 1,
            Country = request.Country,
            City = request.City,
            Street = request.Street,
            BuildingNumber = request.BuildingNumber
        };

        _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Workplace>())).Returns(Task.CompletedTask)
            .Callback<Workplace>(w =>
            {
                w.Id = createdWorkplace.Id;
                w.Country = createdWorkplace.Country;
                w.City = createdWorkplace.City;
                w.Street = createdWorkplace.Street;
                w.BuildingNumber = createdWorkplace.BuildingNumber;
            });

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Country.Should().Be(request.Country);
        result.City.Should().Be(request.City);
        result.Street.Should().Be(request.Street);
        result.BuildingNumber.Should().Be(request.BuildingNumber);
    }

    /// <summary>
    /// Tests the UpdateAsync method of the WorkplaceService, 
    /// ensuring that it updates an existing workplace correctly when the workplace exists.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateWorkplace_WhenWorkplaceExists()
    {
        var existingWorkplace = new Workplace { Id = 1, Country = "Country 1", City = "City 1", Street = "Street 1", BuildingNumber = 1 };
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingWorkplace);

        var request = new ChangeWorkplaceRequest
        {
            Country = "Updated Country",
            City = "Updated City",
            Street = "Updated Street",
            BuildingNumber = 2
        };

        _repoMock.Setup(repo => repo.UpdateAsync(existingWorkplace, 1)).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(request, 1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Country.Should().Be(request.Country);
        result.City.Should().Be(request.City);
        result.Street.Should().Be(request.Street);
        result.BuildingNumber.Should().Be(request.BuildingNumber);
    }

    /// <summary>
    /// Tests the UpdateAsync method of the WorkplaceService, 
    /// ensuring that it returns null when trying to update a workplace that does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenWorkplaceDoesNotExist()
    {
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Workplace?)null);

        var request = new ChangeWorkplaceRequest
        {
            Country = "Updated Country",
            City = "Updated City",
            Street = "Updated Street",
            BuildingNumber = 2
        };

        var result = await _service.UpdateAsync(request, 1);

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests the DeleteAsync method of the WorkplaceService, 
    /// ensuring that it deletes an existing workplace correctly when the workplace exists.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldDeleteWorkplace_WhenWorkplaceExists()
    {
        var existingWorkplace = new Workplace { Id = 1, Country = "Country 1", City = "City 1", 
        Street = "Street 1", BuildingNumber = 1 };
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingWorkplace);

        bool wasDeleted = false;
        _repoMock.Setup(repo => repo.DeleteAsync(1))
                .Callback(() => wasDeleted = true)
                .Returns(Task.CompletedTask);

        
        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
        wasDeleted.Should().BeTrue();
    }

    /// <summary>
    /// Tests the DeleteAsync method of the WorkplaceService, 
    /// ensuring that it returns false when trying to delete a workplace that does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenWorkplaceDoesNotExist()
    {
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Workplace?)null);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();

        var deletedWorkplace = await _service.GetByIdAsync(1);
        deletedWorkplace.Should().BeNull();
    }

 
}