using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Internal;
using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Requests.Vehicle;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.DTOs.Responses.Offer;
using LuxGarage.API.DTOs.Responses.Vehicle;
using LuxGarage.API.DTOs.VehicleImage.Requests;
using LuxGarage.API.DTOs.VehicleImage.Responses;
using LuxGarage.API.Models;

namespace LuxGarage.API.Profiles
{
    /// <summary>
    /// Represents a mapping profile for AutoMapper in the LuxGarage API, 
    /// defining the mappings between domain models and data transfer objects (DTOs) used in the application.
    /// This class serves as a central configuration for AutoMapper, allowing for the mapping of properties
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the MapperProfile class, defining the mappings between domain models and DTOs used in the LuxGarage API.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the mapping configurations for various entities in the application, including employees, vehicles, and workplaces, allowing for the seamless conversion between domain models and DTOs throughout the application.
        /// The mappings defined in this constructor enable the application to manage and display data effectively, ensuring that the appropriate information is transferred between different layers of the application while maintaining a clear separation of concerns.
        /// The mappings include configurations for mapping properties from domain models to response DTOs, as well as from request DTOs to domain models, facilitating the handling of data in both directions within the application.
        /// </remarks>
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

            CreateMap<Workplace, WorkplaceResponse>();
            CreateMap<ChangeWorkplaceRequest, Workplace>();

<<<<<<< HEAD
            // VehicleImage
            CreateMap<VehicleImage, VehicleImageResponse>();
<<<<<<< HEAD

            // Offer
            CreateMap<Offer, OfferListItemResponse>()
                .ForMember(dest => dest.ModelName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleModel.Name))
                .ForMember(dest => dest.Horsepower,
                    opt => opt.MapFrom(src => src.Vehicle.Horsepower))
                .ForMember(dest => dest.BodyName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleBody.Name))
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleBrand.Name))
                .ForMember(dest => dest.Mileage,
                    opt => opt.MapFrom(src => src.Vehicle.Mileage))
                .ForMember(dest => dest.PrimaryImageUrl,
                    opt => opt.MapFrom(src => src.Vehicle.Images[0]))
                .ForMember(dest => dest.IsAvailable,
                    opt => opt.MapFrom(src => src.Status.IsAvailable))
                .ForMember(dest => dest.AvailableFrom,
                    opt => opt.MapFrom(src => src.Status.StartingDate))
                .ForMember(dest => dest.AvailableTo,
                    opt => opt.MapFrom(src => src.Status.DateToEnd));

            CreateMap<Offer, OfferDetailsResponse>()
                .ForMember(dest => dest.ModelName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleModel.Name))
                .ForMember(dest => dest.Horsepower,
                    opt => opt.MapFrom(src => src.Vehicle.Horsepower))
                .ForMember(dest => dest.BodyName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleBody.Name))
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleBrand.Name))
                .ForMember(dest => dest.Mileage,
                    opt => opt.MapFrom(src => src.Vehicle.Mileage))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Prices[0].PricePerDay))
                .ForMember(dest => dest.IsAvailable,
                    opt => opt.MapFrom(src => src.Status.IsAvailable))
                .ForMember(dest => dest.AvailableFrom,
                    opt => opt.MapFrom(src => src.Status.StartingDate))
                .ForMember(dest => dest.AvailableTo,
                    opt => opt.MapFrom(src => src.Status.DateToEnd))
                .ForMember(dest => dest.ColorName,
                    opt => opt.MapFrom(src => src.Vehicle.VehicleColor.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Status.Description))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Vehicle.Images))
                .ForMember(dest => dest.LicensePlate,
                    opt => opt.MapFrom(src => src.Vehicle.LicensePlate));

            this.Internal().ForAllMaps((typeMap, mappingExpression) =>
            {
                mappingExpression.MaxDepth(2);
            }
            );
=======
>>>>>>> 7a207201aebbc6bc3f4224088e18f20b7ac065dc
=======
            CreateMap<UpdateVehicleRequest, Vehicle>()
                     .ForAllMembers(opts => 
                     opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<UpdateCustomerRequest, Customer>()
                     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Customer, CustomerResponse>();
            CreateMap<Rental, RentalResponse>();
            CreateMap<CreateRentalRequest, Rental>();
>>>>>>> cf672632a7e9d9291ea91ad30ebb79504d978c9f
        }
    }
}
