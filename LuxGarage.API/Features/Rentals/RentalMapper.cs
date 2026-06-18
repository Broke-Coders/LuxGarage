using AutoMapper;
using LuxGarage.API.Models;

namespace LuxGarage.API.Features.Rentals;

public class RentalMapper : Profile
{
    public RentalMapper()
    {
        CreateMap<Rental, RentalResponse>()
            .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.Brand))
            .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
            .ForMember(dest => dest.VehicleLicensePlate, opt => opt.MapFrom(src => src.Vehicle.LicensePlate))
            .ForMember(dest => dest.VehicleYear, opt => opt.MapFrom(src => src.Vehicle.Year))
            .ForMember(dest => dest.VehicleImageUrl, opt => opt.MapFrom(src => $"/api/VehicleImages/vehicle/{src.VehicleId}/primary"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
