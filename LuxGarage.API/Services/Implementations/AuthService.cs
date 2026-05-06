using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _employeeRepository.GetByLoginAsync(request.Login);
            if (existingUser != null) return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var employee = new Employee
            {
                Login = request.Login,
                Password = hashedPassword,
                WorkplaceId = request.WorkplaceId,
                PermissionId = request.PermissionId
            };

            await _employeeRepository.AddAsync(employee);
            return true;
        }
    }
}
