using LuxGarage.API.Models;
using LuxGarage.API.Features.Users;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Employee instances.
/// </summary>
public class EmployeeBuilder
{
    private string _email = "employee@example.com";
    private string _passwordHash = "hashedpassword";
    private string _firstName = "Jane";
    private string _lastName = "Smith";
    private UserRole _role = UserRole.Employee;
    private bool _isActive = true;
    private int _workplaceId = 1;

    public EmployeeBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public EmployeeBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public EmployeeBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public EmployeeBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public EmployeeBuilder WithRole(UserRole role)
    {
        _role = role;
        return this;
    }

    public EmployeeBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public EmployeeBuilder WithWorkplaceId(int workplaceId)
    {
        _workplaceId = workplaceId;
        return this;
    }

    public Employee Build() => new Employee
    {
        Email = _email,
        PasswordHash = _passwordHash,
        FirstName = _firstName,
        LastName = _lastName,
        Role = _role,
        IsActive = _isActive,
        WorkplaceId = _workplaceId
    };
}
