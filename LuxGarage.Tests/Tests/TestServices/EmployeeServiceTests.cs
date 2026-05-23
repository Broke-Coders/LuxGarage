using AutoMapper;
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
/// Tests for the EmployeeService class, which provides business logic for managing Employee entities.
/// </summary>
public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<IWorkplaceRepository> _workplaceRepoMock;
    private readonly Mock<IPermissionRepository> _permissionRepoMock;
    private readonly IMapper _mapper;
    private readonly IEmployeeService _service;

    /// <summary>
    /// Constructor for the EmployeeServiceTests class, which initializes the test class with a mock repository and an AutoMapper instance.
    /// </summary>
    public EmployeeServiceTests()
    {
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _workplaceRepoMock = new Mock<IWorkplaceRepository>();
        _permissionRepoMock = new Mock<IPermissionRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new EmployeeService(_employeeRepoMock.Object, _mapper, _workplaceRepoMock.Object, _permissionRepoMock.Object);
    }

    /// <summary>
    /// Tests that retrieving all employees returns an empty list when no employees exist in the repository,
    /// indicating that the service correctly handles the case where there are no employees to retrieve and returns 
    /// an empty list in the response.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
    {
        _employeeRepoMock.Setup(repo => repo.GetAllAsync())
                 .ReturnsAsync(new List<Employee>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that retrieving all employees returns a list of employees when employees exist in the repository,
    /// indicating that the service correctly retrieves and maps employee data from the repository to the response DTOs, 
    /// and that the returned list contains the expected employee information.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmployees_WhenEmployeesExist()
    {
        var employees = new List<Employee>
        {
            new Employee { Id = 1, Login = "jdoe", Password = "password123" },
            new Employee { Id = 2, Login = "jsmith", Password = "password456" }
        };

        _employeeRepoMock.Setup(repo => repo.GetAllAsync())
                 .ReturnsAsync(employees);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(e => e.Id == 1 && e.Login == "jdoe");
        result.Should().Contain(e => e.Id == 2 && e.Login == "jsmith");
    }


    /// <summary>
    /// Tests that an employee can be retrieved by their ID and that the correct details are returned when the employee exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };

        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Login.Should().Be("jdoe");
    }


    /// <summary>
    /// Tests that updating an existing employee with valid data returns the updated employee details,
    /// indicating that the employee was successfully updated and the correct information is returned in the response.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedEmployee_WhenEmployeeExists()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };
        var workplace = new Workplace { Id = 1, Country = "USA", City = "New York", Street = "5th Avenue", BuildingNumber = 5 };
        var permission = new Permission { Id = 1, Name = "Admin" };

        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);
        _workplaceRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(workplace);
        _permissionRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(permission);

        var request = new UpdateEmployeeRequest { Login = "newlogin", WorkplaceId = 1, PermissionId = 1 };

        var result = await _service.UpdateAsync(1, request);

        result.Should().NotBeNull();
        result!.Login.Should().Be("newlogin");
    }

    /// <summary>
    /// Tests that attempting to update an employee with a workplace ID that does not exist throws an InvalidOperationException,
    /// indicating that the workplace with the specified ID was not found and therefore could not be 
    /// assigned to the employee during the update process.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenWorkplaceNotFound()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };

        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);

        var request = new UpdateEmployeeRequest { Login = "newlogin", WorkplaceId = 9999, PermissionId = 1 };

        Func<Task> act = async () => await _service.UpdateAsync(1, request);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Workplace with ID 9999 does not exists.");
    }

    /// <summary>
    /// Tests that attempting to update an employee with a permission ID that does not exist throws an InvalidOperationException,
    /// indicating that the permission with the specified ID was not found and therefore could not be assigned 
    /// to the employee during the update process.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenPermissionNotFound()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };
        var workplace = new Workplace { Id = 1, Country = "USA", City = "New York", Street = "5th Avenue", BuildingNumber = 5 };
        var permission = new Permission { Id = 1, Name = "Admin" };

        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);
        _workplaceRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(workplace);
        _permissionRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(permission);

        var request = new UpdateEmployeeRequest { Login = "newlogin", WorkplaceId = 1, PermissionId = 9999 };

        Func<Task> act = async () => await _service.UpdateAsync(1, request);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Permission with ID 9999 does not exists.");
    }

    /// <summary>
    /// Tests that attempting to update an employee that does not exist throws an InvalidOperationException,
    /// indicating that the employee with the specified ID was not found and therefore could not be updated
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenIdNotFound()
    {
        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((Employee?)null!);
        var request = new UpdateEmployeeRequest { Login = "newlogin", WorkplaceId = 1, PermissionId = 1 };
        Func<Task> act = async () => await _service.UpdateAsync(9999, request);
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage("Employee with ID 9999 does not exists.");
    }

    /// <summary>
    /// Tests that attempting to delete an employee that does not exist returns false, 
    /// indicating that the employee was not found and therefore not deleted.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEmployeeNotFound()
    {
        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((Employee?)null!);

        var result = await _service.DeleteAsync(9999);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that deleting an existing employee returns true, indicating that the employee was successfully deleted.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenEmployeeDeleted()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };

        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that attempting to change the password of an employee that does not exist returns false, 
    /// indicating that the password change was unsuccessful.
    /// </summary>
    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFalse_WhenEmployeeNotFound()
    {
        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((Employee?)null!);

        var request = new ChangePasswordRequest { NewPassword = "newpass" };

        var result = await _service.ChangePasswordAsync(9999, request);

        result.Should().BeFalse();
    }


    /// <summary>
    /// Tests that changing the password of an existing employee returns true, indicating that the password was successfully changed.
    /// </summary>
    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnTrue_WhenPasswordChanged()
    {
        var employee = new Employee { Id = 1, Login = "jdoe", Password = "password123" };
        _employeeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(employee);
        var request = new ChangePasswordRequest { NewPassword = "newpass" };
        var result = await _service.ChangePasswordAsync(1, request);
        result.Should().BeTrue();
    }


}