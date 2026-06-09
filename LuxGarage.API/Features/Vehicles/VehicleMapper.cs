using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Vehicles;

public class VehicleMapper : Profile
{
    public VehicleMapper()
    {
        CreateMap<Vehicle, VehicleResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}