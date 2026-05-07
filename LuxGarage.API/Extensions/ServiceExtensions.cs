using System;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Implementations;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleBrandRepository, VehicleBrandRepository>();
        services.AddScoped<IVehicleModelRepository, VehicleModelRepository>();
        services.AddScoped<IVehicleBodyRepository, VehicleBodyRepository>();
        services.AddScoped<IVehicleColorRepository, VehicleColorRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRentalInsuranceRepository, RentalInsuranceRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();

        return services;
    }


    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IWorkplaceService, WorkplaceService>();

        return services;
    }
}
