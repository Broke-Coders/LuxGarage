using AutoMapper;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Profiles;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Implementations;
using LuxGarage.API.Services.Interfaces;
using Moq;
using Xunit;

namespace LuxGarage.Tests.TestServices;

/// <summary>
/// Unit tests for AuthService
/// </summary>
/// <remarks>
/// This class contains unit tests for the AuthService, which handles user registration and authentication.
/// The tests cover covering both RegisterAsync and LoginAsync methods, 
/// ensuring proper handling of user registration and authentication scenarios, 
/// including edge cases such as existing users and invalid credentials.
/// The tests utilize Moq to mock the IEmployeeRepository and FluentAssertions for expressive assertions.
/// The configuration for JWT token generation is also set up in-memory for testing purposes.
/// </remarks>
public class AuthServiceTest
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly IAuthService _service;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the AuthServiceTest class, 
    /// setting up the necessary mocks and configuration for testing.
    /// </summary>
    public AuthServiceTest()
    {
        _repoMock = new Mock<IEmployeeRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });

        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:Key", "ThisIsASecretKeyForJwtTokenGeneration256"}};

        _configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings!)
                            .Build();

        _service = new AuthService(_repoMock.Object, _configuration);
    }

    /// <summary>
    /// Tests the RegisterAsync method to ensure that it throws an InvalidOperationException
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenUserAlreadyExists()
    {
        var existingEmployee = new Employee { Id = 1, Login = "existingUser", Password = "hashedPassword" };
        _repoMock.Setup(repo => repo.GetByLoginAsync("existingUser"))
                 .ReturnsAsync(existingEmployee);

        var request = new RegisterRequest
        {
            Login = "existingUser",
            Password = "password123",
            WorkplaceId = 1,
            PermissionId = 1
        };

        Func<Task> act = async () => await _service.RegisterAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("User existingUser already exists.");
        _repoMock.Verify(repo => repo.GetByLoginAsync("existingUser"), Times.Once);
        _repoMock.Verify(repo => repo.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    /// <summary>
    /// Tests the RegisterAsync method to ensure that it successfully registers a new user 
    /// and returns the correct details.
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ShouldReturnRegisteredEmployeeId()
    {
        var newEmployee = new Employee { Id = 2, Login = "newUser", Password = "hashedPassword" };
        _repoMock.Setup(repo => repo.GetByLoginAsync("newUser"))
                 .ReturnsAsync((Employee)null!);
        
        _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                    .Callback<Employee>(emp => emp.Id = newEmployee.Id)
                    .Returns(Task.CompletedTask);

        var result = await _service.RegisterAsync(new RegisterRequest
        {
            Login = "newUser",
            Password = "password123",
            WorkplaceId = 1,
            PermissionId = 1
        });

        result.Should().NotBeNull();
        result.Id.Should().Be(2);
        result.Login.Should().Be("newUser");
    }

    /// <summary>
    /// Tests that the LoginAsync method returns null 
    /// when a user with the provided login does not exist in the repository.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _repoMock.Setup(repo => repo.GetByLoginAsync("nonExistentUser"))
                 .ReturnsAsync((Employee)null!);

        var result = await _service.LoginAsync(new LoginRequest
        {
            Login = "nonExistentUser",
            Password = "password123"
        });

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that the LoginAsync method returns null when the provided password is 
    /// invalid for an existing user.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        var employee = new Employee { Id = 1, Login = "validUser", Password = BCrypt.Net.BCrypt.HashPassword("correctPassword") };
        _repoMock.Setup(repo => repo.GetByLoginAsync("validUser"))
                 .ReturnsAsync(employee);

        var result = await _service.LoginAsync(new LoginRequest
        {
            Login = "validUser",
            Password = "wrongPassword"
        });

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that the LoginAsync method returns a valid login response when 
    /// the provided credentials are correct.
    /// </summary>
    [Fact]
    public async Task LoginAsync_ShouldReturnLoginResponse_WhenCredentialsAreValid()
    {
        var employee = new Employee { Id = 1, Login = "validUser", Password = BCrypt.Net.BCrypt.HashPassword("correctPassword"), Permission = new Permission { Name = "Admin" } };
        _repoMock.Setup(repo => repo.GetByLoginAsync("validUser"))
                 .ReturnsAsync(employee);

        var result = await _service.LoginAsync(new LoginRequest
        {
            Login = "validUser",
            Password = "correctPassword"
        });

        result.Should().NotBeNull();
        result!.Login.Should().Be("validUser");
        result.Role.Should().Be("Admin");
        result.Token.Should().NotBeNullOrEmpty();
    }
                    
}