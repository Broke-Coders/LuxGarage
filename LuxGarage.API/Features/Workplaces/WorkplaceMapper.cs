using AutoMapper;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LuxGarage.API.Features.Workplaces;

public class WorkplaceMapper : Profile
{
    public WorkplaceMapper()
    {
        CreateMap<Workplace, WorkplaceResponse>();
    }
}