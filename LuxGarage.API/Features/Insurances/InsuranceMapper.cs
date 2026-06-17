using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Insurances;

public class InsuranceMapper : Profile
{
    public InsuranceMapper()
    {
        CreateMap<Insurance, InsuranceResponse>();
    }
}