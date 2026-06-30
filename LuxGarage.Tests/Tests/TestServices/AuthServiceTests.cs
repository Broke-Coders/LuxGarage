using System;
using System.Threading.Tasks;
using FluentAssertions;
using LuxGarage.API.Features.Auth;
using LuxGarage.API.Models;
using LuxGarage.Tests.Bases;
using LuxGarage.Tests.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace LuxGarage.Tests.TestServices;

public class AuthServiceTests : ServiceTestBase
{
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IConfiguration> _configurationMock;

    public AuthServiceTests(SharedDatabaseFixture fixture) : base(fixture)
    {
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _configurationMock = new Mock<IConfiguration>();
    }

    private AuthService Service => new AuthService(context, _configurationMock.Object, _passwordHasherMock.Object);

    [Theory]
    [InlineData("antigra", "Password is too short.")]
    [InlineData("anti1234", "Missing an uppercase letter.")]
    [InlineData("Antianti", "Missing a digit.")]
    public async Task RegisterAsync_ShouldThrowArgumentException_WhenPasswordDoesNotMeetRequirements(string invalidPassword, string expectedErrorMessage)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@luxgarage.com",
            Password = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            IsEmployee = false
        };

        // Act
        Func<Task> act = async () => await Service.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage(expectedErrorMessage);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterSuccessfully_WhenPasswordIsValid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "newuser@luxgarage.com",
            Password = "ValidPassword123",
            FirstName = "Test",
            LastName = "User",
            IsEmployee = false,
            PhoneNumber = "123456789",
            LicenseNumber = "LIC123"
        };

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
            .Returns("hashed_password");

        // Act
        var result = await Service.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
    }
}
