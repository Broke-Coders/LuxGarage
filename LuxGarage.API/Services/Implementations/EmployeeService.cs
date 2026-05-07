using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkplaceRepository _workplaceRepository;
        private readonly IPermissionRepository _permissionRepository;

        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IWorkplaceRepository workplaceRepository, IPermissionRepository permissionRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _workplaceRepository = workplaceRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"Employee with ID {id} does not exists.");

            var workplace = await _workplaceRepository.GetByIdAsync(request.WorkplaceId)
                            ?? throw new InvalidOperationException($"Workplace with ID {request.WorkplaceId} does not exists.");

            var permission = await _permissionRepository.GetByIdAsync(request.PermissionId)
                             ?? throw new InvalidOperationException($"Permission with ID {request.PermissionId} does not exists.");

            _mapper.Map(request, employee);

            await _employeeRepository.UpdateAsync(id, employee);

            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            await _employeeRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            employee.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _employeeRepository.UpdateAsync(id, employee);
            return true;

        }
    }
}
