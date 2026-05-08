using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LuxGarage.API.Services.Implementations
{
    /// <summary>
    /// Implements the authentication service for the LuxGarage API, providing methods for user registration and login,
    /// including password hashing and JWT token generation for authenticated users.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the AuthService class with the specified employee repository and configuration,
        /// allowing for dependency injection of the repository and access to configuration settings for JWT token generation.
        /// </summary>
        /// <param name="employeeRepository">Employee repository to interact with the database.</param>
        /// <param name="configuration">Configuration settings for JWT token generation.</param>
        public AuthService(IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user with the specified login and password.
        /// </summary>
        /// <param name="request">The registration request containing login and password.</param>
        /// <returns>The response containing the registered user's information.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a user with the specified login already exists.</exception>
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _employeeRepository.GetByLoginAsync(request.Login);
            if (existingUser != null) throw new InvalidOperationException($"User {request.Login} already exists.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var employee = new Employee
            {
                Login = request.Login,
                Password = hashedPassword,
                WorkplaceId = request.WorkplaceId,
                PermissionId = request.PermissionId
            };

            await _employeeRepository.AddAsync(employee);
            return new RegisterResponse{Id = employee.Id, Login = employee.Login};
        }

        /// <summary>
        /// Authenticates a user with the specified login and password.
        /// </summary>
        /// <param name="request">The login request containing login and password.</param>
        /// <returns>The response containing the authenticated user's information, or null if authentication fails.</returns>
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _employeeRepository.GetByLoginAsync(request.Login);
            if (user == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            
            if (!isPasswordValid) return null;

            string token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Login = user.Login,
                Role = user.Permission.Name ?? "Worker"
            };

        }

        /// <summary>
        /// Generates a JWT token for the authenticated user, including claims for user ID, login, role, and workplace ID,
        /// and signing the token with a symmetric security key from the configuration settings. The token is set to expire after 8 hours.
        /// </summary>
        /// <param name="employee">The employee for whom to generate a JWT token.</param>
        /// <returns>The generated JWT token.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the JWT key is missing in configuration.</exception>
        private string GenerateJwtToken(Employee employee)
        {
            var keyString = _configuration["JwtSettings:Key"] 
                            ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
            var key = Encoding.ASCII.GetBytes(keyString);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                    new Claim(ClaimTypes.Name, employee.Login),
                    new Claim(ClaimTypes.Role, employee.Permission.Name ?? "Worker"),
                    new Claim("WorkplaceId", employee.WorkplaceId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
