using LuxGarage.API.Models;
using LuxGarage.API.Features.Users;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Customer instances.
/// </summary>
public class CustomerBuilder
{
    private string _email = "customer@example.com";
    private string _passwordHash = "hashedpassword";
    private string _firstName = "John";
    private string _lastName = "Doe";
    private UserRole _role = UserRole.Customer;
    private bool _isActive = true;
    private string _phoneNumber = "123456789";
    private string _licenseNumber = "ABC12345";
    private int _borrowCounter = 0;

    public CustomerBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CustomerBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public CustomerBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public CustomerBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public CustomerBuilder WithRole(UserRole role)
    {
        _role = role;
        return this;
    }

    public CustomerBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public CustomerBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public CustomerBuilder WithLicenseNumber(string licenseNumber)
    {
        _licenseNumber = licenseNumber;
        return this;
    }

    public CustomerBuilder WithBorrowCounter(int borrowCounter)
    {
        _borrowCounter = borrowCounter;
        return this;
    }

    public Customer Build() => new Customer
    {
        Email = _email,
        PasswordHash = _passwordHash,
        FirstName = _firstName,
        LastName = _lastName,
        Role = _role,
        IsActive = _isActive,
        PhoneNumber = _phoneNumber,
        LicenseNumber = _licenseNumber,
        BorrowCounter = _borrowCounter
    };
}
