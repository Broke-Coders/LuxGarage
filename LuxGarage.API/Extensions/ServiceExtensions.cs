using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LuxGarage.API.Models;
using Microsoft.AspNetCore.Identity;
using LuxGarage.API.Features.Auth;
using LuxGarage.API.Features.Offers;
using LuxGarage.API.Features.Rentals;
using LuxGarage.API.Features.Users;
using LuxGarage.API.Features.Vehicles;
using LuxGarage.API.Features.Workplaces;
using Microsoft.OpenApi;

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
    /// Rejestruje wszystkie pionowe serwisy z Vertical Slices w kontenerze DI.
    /// Zrezygnowano ze wzorca Repository, co eliminuje konieczność rejestracji interfejsów danych.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        
        services.AddScoped<IPricingStrategy, LongTermDiscountStrategy>();
        services.AddScoped<IPricingStrategy, WeekendSurchargeStrategy>();
        services.AddScoped<DynamicPricingEngine>();

        services.AddScoped<AuthService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<WorkplaceService>();
        services.AddScoped<VehicleImageService>();
        services.AddScoped<VehicleService>();
        services.AddScoped<OfferService>();
        services.AddScoped<RentalService>();

        return services;
    }

    /// <summary>
    /// Konfiguruje mechanizm sprawdzania tokenów JWT wysyłanych przez frontend.
    /// Bez tego API nie będzie potrafiło rozszyfrować faktu, czy dany request pochodzi od zalogowanego usera.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var keyString = configuration["JwtSettings:Key"];
        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException("JWT Key is missing in appsettings.json.");

        var key = Encoding.ASCII.GetBytes(keyString);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; 
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                
                ValidateIssuer = false, 
                ValidateAudience = false,
                ValidateLifetime = true, 
                
                ClockSkew = TimeSpan.Zero 
            };
        });

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

    public static IServiceCollection AddSwaggerWithJwtAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });

        return services;
    }
}
