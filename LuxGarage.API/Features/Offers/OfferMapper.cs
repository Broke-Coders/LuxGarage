using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Offers;

public class OfferMapper : Profile
{
    public OfferMapper()
    {
        CreateMap<Offer, OfferListItemResponse>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Vehicle.Brand))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Vehicle.Model))
            .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Vehicle.Mileage))
            .ForMember(dest => dest.PrimaryImageUrl, opt => opt.Ignore());

        CreateMap<Offer, OfferDetailsResponse>()
            .IncludeBase<Offer, OfferListItemResponse>()
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Vehicle.Year))
            .ForMember(dest => dest.Horsepower, opt => opt.MapFrom(src => src.Vehicle.Horsepower))

            .ForMember(dest => dest.BodyType, opt => opt.MapFrom(src => src.Vehicle.BodyType.ToString()))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Vehicle.Color.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Vehicle.Status.ToString()));
    }
}