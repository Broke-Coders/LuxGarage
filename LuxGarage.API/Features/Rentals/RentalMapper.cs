using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Rentals;

public class RentalMapper : Profile
{
    public RentalMapper()
    {
        CreateMap<Rental, RentalResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}