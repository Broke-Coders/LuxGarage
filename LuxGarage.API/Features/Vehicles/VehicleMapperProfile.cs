using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Vehicles;

public class VehicleMapperProfile : Profile
{
    public VehicleMapperProfile()
    {
        CreateMap<Vehicle, VehicleResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}