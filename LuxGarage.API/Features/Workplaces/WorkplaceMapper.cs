using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Workplaces;

public class WorkplaceMapper : Profile
{
    public WorkplaceMapper()
    {
        CreateMap<Workplace, WorkplaceResponse>();
    }
}