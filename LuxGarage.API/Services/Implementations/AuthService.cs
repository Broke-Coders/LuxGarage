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
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

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
