using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LuxGarage.API.Data;
using LuxGarage.API.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Auth;

public class AuthService
{
    private readonly RentalContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(RentalContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {request.Email} already exists.");

        User newUser;

        if (request.IsEmployee)
        {
            if (request.WorkplaceId == null)
                throw new ArgumentException("WorkplaceId is required when creating an employee.");

            newUser = new Employee
            {
                Email = request.Email,
                PasswordHash = string.Empty, 
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = UserRole.Employee,
                Status = EmployeeStatus.Pending,
                WorkplaceId = request.WorkplaceId.Value
            };
        }
        else
        {
            newUser = new Customer
            {
                Email = request.Email,
                PasswordHash = string.Empty,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = UserRole.Customer,
                PhoneNumber = request.PhoneNumber ?? "N/A",
                LicenseNumber = request.LicenseNumber ?? "N/A"
            };
        }

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            Id = newUser.Id,
            Email = newUser.Email,
            Role = newUser.Role.ToString()
        };
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null) 
            throw new ArgumentException("User with given email does not exist");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        
        if (user == null || verificationResult == PasswordVerificationResult.Failed)
            throw new ArgumentException("Invalid password");

        if (user.Role == UserRole.Employee)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == request.Email);

            if (employee != null && employee.Status == EmployeeStatus.Pending)
                throw new UnauthorizedAccessException("Your employee account is waiting for admin approval");
        }

        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    private string GenerateJwtToken(User user)
    {
        var keyString = _configuration["JwtSettings:Key"] 
                        ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
        
        var key = Encoding.ASCII.GetBytes(keyString);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        if (user is Employee employee)
        {
            claims.Add(new Claim("WorkplaceId", employee.WorkplaceId.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}