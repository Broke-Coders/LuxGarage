using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations
{
    /// <summary>
    /// Implements the workplace service for the LuxGarage API, providing methods for managing workplace data, 
    /// including retrieval, creation, updating, and deletion of workplaces, while ensuring proper validation
    /// </summary>
    public class WorkplaceService : IWorkplaceService
    {
        private readonly IWorkplaceRepository _workplaceRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the WorkplaceService class with the specified workplace repository and mapper.
        /// </summary>
        /// <param name="workplaceRepository">The workplace repository to use.</param>
        /// <param name="mapper">The mapper to use.</param>
        public WorkplaceService(IWorkplaceRepository workplaceRepository, IMapper mapper)
        {
            _workplaceRepository = workplaceRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all workplaces from the repository and maps them to a collection of WorkplaceResponse DTOs
        /// </summary>
        /// <returns>A collection of WorkplaceResponse DTOs.</returns>
        public async Task<IEnumerable<WorkplaceResponse>> GetAllAsync()
        {
            var workplaces = await _workplaceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<WorkplaceResponse>>(workplaces);
        }

        /// <summary>
        /// Retrieves a workplace by its ID from the repository and maps it to a WorkplaceResponse DTO.
        /// </summary>
        /// <param name="id">The ID of the workplace to retrieve.</param>
        /// <returns>The WorkplaceResponse DTO.</returns>
        public async Task<WorkplaceResponse?> GetByIdAsync(int id)
        {
            var workplace = await _workplaceRepository.GetByIdAsync(id);
            return _mapper.Map<WorkplaceResponse>(workplace);
        }

        /// <summary>
        /// Creates a new workplace based on the provided request, mapping the request to a Workplace model, adding it to the repository,
        /// and returning the created workplace as a WorkplaceResponse DTO.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<WorkplaceResponse> CreateAsync(ChangeWorkplaceRequest request)
        {
            var workplace = _mapper.Map<Workplace>(request);
            await _workplaceRepository.AddAsync(workplace);
            return _mapper.Map<WorkplaceResponse>(workplace);
        }

        /// <summary>
        /// Updates an existing workplace based on the provided request and ID, retrieving the existing workplace from the repository,
        /// mapping the request to the existing workplace model, updating it in the repository, and returning
        /// </summary>
        /// <param name="request">The request containing the updated workplace information.</param>
        /// <param name="id">The ID of the workplace to update.</param>
        /// <returns>The updated WorkplaceResponse DTO, or null if the workplace is not found.</returns>
        public async Task<WorkplaceResponse?> UpdateAsync(ChangeWorkplaceRequest request, int id)
        {
            var existingWorkplace = await _workplaceRepository.GetByIdAsync(id);
            if (existingWorkplace == null) return null;

            _mapper.Map(request, existingWorkplace);
            await _workplaceRepository.UpdateAsync(existingWorkplace, id);

            return _mapper.Map<WorkplaceResponse>(existingWorkplace);
        }

        /// <summary>
        /// Deletes a workplace by its ID, first checking if the workplace exists in the repository, 
        /// and if it does, deleting it and returning true; otherwise, returning false.
        /// </summary>
        /// <param name="id">The ID of the workplace to delete.</param>
        /// <returns>true if the workplace was deleted, false otherwise.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var workplace = await _workplaceRepository.GetByIdAsync(id);
            if(workplace == null) return false;

            await _workplaceRepository.DeleteAsync(id);
            return true;
        }
    }
}
