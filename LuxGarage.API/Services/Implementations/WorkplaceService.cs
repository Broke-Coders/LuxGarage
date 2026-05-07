using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Services.Implementations
{
    public class WorkplaceService : IWorkplaceService
    {
        private readonly IWorkplaceRepository _workplaceRepository;
        private readonly IMapper _mapper;

        public WorkplaceService(IWorkplaceRepository workplaceRepository, IMapper mapper)
        {
            _workplaceRepository = workplaceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkplaceResponse>> GetAllAsync()
        {
            var workplaces = await _workplaceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<WorkplaceResponse>>(workplaces);
        }

        public async Task<WorkplaceResponse?> GetByIdAsync(int id)
        {
            var workplace = await _workplaceRepository.GetByIdAsync(id);
            return _mapper.Map<WorkplaceResponse>(workplace);
        }

        public async Task<WorkplaceResponse> CreateAsync(ChangeWorkplaceRequest request)
        {
            var workplace = _mapper.Map<Workplace>(request);
            await _workplaceRepository.AddAsync(workplace);
            return _mapper.Map<WorkplaceResponse>(workplace);
        }

        public async Task<WorkplaceResponse?> UpdateAsync(ChangeWorkplaceRequest request, int id)
        {
            var existingWorkplace = await _workplaceRepository.GetByIdAsync(id);
            if (existingWorkplace == null) return null;

            _mapper.Map(request, existingWorkplace);
            await _workplaceRepository.UpdateAsync(existingWorkplace, id);

            return _mapper.Map<WorkplaceResponse>(existingWorkplace);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workplace = await _workplaceRepository.GetByIdAsync(id);
            if(workplace == null) return false;

            await _workplaceRepository.DeleteAsync(id);
            return true;
        }
    }
}
