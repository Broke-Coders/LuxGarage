using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations
{
    /// <summary>
    /// Implements the employee service for the LuxGarage API, providing methods for managing employee data, 
    /// including retrieval, updating, deletion, and password changes, while ensuring proper validation 
    /// and mapping of data transfer objects to the underlying data models.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkplaceRepository _workplaceRepository;
        private readonly IPermissionRepository _permissionRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the EmployeeService class with the specified repositories and mapper.
        /// </summary>
        /// <param name="employeeRepository">The repository for managing employee data.</param>
        /// <param name="mapper">The mapper for converting between data transfer objects and data models.</param>
        /// <param name="workplaceRepository">The repository for managing workplace data.</param>
        /// <param name="permissionRepository">The repository for managing permission data.</param>
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IWorkplaceRepository workplaceRepository, IPermissionRepository permissionRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _workplaceRepository = workplaceRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Retrieves all employees from the repository and maps them to a collection of EmployeeResponse DTOs.
        /// </summary>
        /// <returns>A collection of EmployeeResponse DTOs.</returns>
        public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
        }

        /// <summary>
        /// Retrieves an employee by their ID from the repository and maps it to an EmployeeResponse DTO.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The EmployeeResponse DTO, or null if the employee is not found.</returns>
        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            return _mapper.Map<EmployeeResponse>(employee);
        }

        /// <summary>
        /// Updates an existing employee's information based on the provided ID and update request, 
        /// ensuring that the associated workplace and permission exist, and returns the updated employee as an EmployeeResponse DTO.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="request">The update request containing the new employee information.</param>
        /// <returns>The updated EmployeeResponse DTO, or null if the employee is not found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the employee with the specified ID is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the associated workplace or permission is not found.</exception>
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

        /// <summary>
        /// Deletes an employee from the repository based on the provided ID, 
        /// returning true if the deletion was successful, or false if the employee was not found.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>true if the deletion was successful, or false if the employee was not found.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            await _employeeRepository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Changes the password for an employee with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the employee whose password to change.</param>
        /// <param name="request">The request containing the new password.</param>
        /// <returns>true if the password was changed successfully, or false if the employee was not found.</returns>
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
