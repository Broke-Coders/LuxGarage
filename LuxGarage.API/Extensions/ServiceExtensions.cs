using System;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Implementations;
using LuxGarage.API.Repositories.Interfaces;
using LuxGarage.API.Services.Implementations;
using LuxGarage.API.Services.Interfaces;

namespace LuxGarage.API.Extensions;

/// <summary>
/// Extension methods for configuring services in the dependency injection container.
/// </summary>
/// <remarks>
/// This class contains extension methods for adding various services and repositories to the service collection,
/// including methods for adding repositories, services, and CORS policies to the service collection used by the application. 
/// These methods help to organize and centralize the configuration of services and dependencies in the application startup process.
/// </remarks>
public static class ServiceExtensions
{
    /// <summary>
    /// Adds repository services to the service collection, including implementations for vehicle, customer, 
    /// insurance, permission, rental, employee, and workplace repositories.
    /// </summary>
    /// <param name="services">The service collection to which the repositories will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleBrandRepository, VehicleBrandRepository>();
        services.AddScoped<IVehicleModelRepository, VehicleModelRepository>();
        services.AddScoped<IVehicleBodyRepository, VehicleBodyRepository>();
        services.AddScoped<IVehicleColorRepository, VehicleColorRepository>();
        services.AddScoped<IVehiclePriceRepository, VehiclePriceRepository>();
        services.AddScoped<IVehicleImageRepository, VehicleImageRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRentalInsuranceRepository, RentalInsuranceRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();

        return services;
    }

    /// <summary>
    /// Adds service implementations to the service collection, including services for vehicle management, authentication, 
    /// employee management, and workplace management, allowing these services to be injected and used throughout the application where needed.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IWorkplaceService, WorkplaceService>();
        services.AddScoped<IVehicleImageService, VehicleImageService>();

        return services;
    }

    /// <summary>
    /// Adds a CORS policy to the service collection that allows any origin, method, and header, 
    /// enabling cross-origin requests to the API from any client or domain.
    /// </summary>
    /// <param name="services">The service collection to which the CORS policy will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCorsPolicy (this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}
