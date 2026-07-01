using AutoMapper;
using LuxGarage.API.Models; 

namespace LuxGarage.API.Features.Users;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<Customer, CustomerResponse>();

        CreateMap<Employee, EmployeeResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.WorkplaceName, opt => opt.MapFrom(src => 
                src.Workplace != null ? $"{src.Workplace.City}, {src.Workplace.Street}" : null));
    }
}