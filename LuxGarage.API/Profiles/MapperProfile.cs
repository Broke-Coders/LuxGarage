using AutoMapper;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.Models;

namespace LuxGarage.API.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(dest => dest.PermissionName,
                    opt => opt.MapFrom(src => src.Permission.Name))
                .ForMember(dest => dest.WorkplaceCity,
                    opt => opt.MapFrom(src => src.Workplace.City));

            CreateMap<RegisterRequest, Employee>();
            CreateMap<UpdateEmployeeRequest, Employee>();

            CreateMap<Vehicle, VehicleListItemResponse>()
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.VehicleBrand.Name))
                .ForMember(dest => dest.ModelName,
                    opt => opt.MapFrom(src => src.VehicleModel.Name))
                .ForMember(dest => dest.BodyName,
                    opt => opt.MapFrom(src => src.VehicleBody.Name))
                .ForMember(dest => dest.ColorName,
                    opt => opt.MapFrom(src => src.VehicleColor.Name));

            CreateMap<Vehicle, VehicleDetailsResponse>()
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.VehicleBrand.Name))
                .ForMember(dest => dest.ModelName,
                    opt => opt.MapFrom(src => src.VehicleModel.Name))
                .ForMember(dest => dest.BodyName,
                    opt => opt.MapFrom(src => src.VehicleBody.Name))
                .ForMember(dest => dest.ColorName,
                    opt => opt.MapFrom(src => src.VehicleColor.Name));

            CreateMap<CreateVehicleRequest, Vehicle>();
        }
    }
}
