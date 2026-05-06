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
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
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
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            _mapper.Map(request, employee);

            await _employeeRepository.UpdateAsync(employee, id);

            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if(employee == null) return false;

            await _employeeRepository.DeleteAsync(id);
            return true;
        }
    }
}
