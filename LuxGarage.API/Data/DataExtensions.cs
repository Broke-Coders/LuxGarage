using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;
using Microsoft.AspNetCore.Identity;
using LuxGarage.API.Features.Users;
using LuxGarage.API.Features.Vehicles;

namespace LuxGarage.API.Data;
/// <summary>
/// Extension methods for database initialization and migration, providing a way to set up the database schema and seed initial data.
/// </summary>
public static class DataExtensions
{
    /// <summary>
    /// Initializes the database schema and seeds initial data if necessary. 
    /// </summary>
    /// <remarks>
    /// This method ensures that the database is created and up to date with the latest migrations, 
    /// and it populates the database with default values for permissions, workplaces, employees, vehicle brands, models, bodies, colors, 
    /// vehicles, insurances, and customers.
    /// </remarks>
    /// <param name="app">The web application instance.</param>
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<RentalContext>();


        dbContext.Database.EnsureDeleted(); // Reseting database  
        dbContext.Database.CloseConnection();
        dbContext.Database.Migrate();

        if (!dbContext.Workplaces.Any())
        {
            dbContext.Workplaces.AddRange(
                new Workplace {Country = "Poland", City = "Krakow", Street = "Bracka", BuildingNumber = "12C"},
                new Workplace {Country = "Poland", City = "Warsaw", Street = "Korfantego", BuildingNumber = "128"}
            );
            dbContext.SaveChanges();
        }

        if (!dbContext.Users.Any())
        {
            var hasher = new PasswordHasher<User>();
            var krakowBranch = dbContext.Workplaces.First(b => b.City == "Krakow");

            var admin = new Employee
            {
                Email = "admin@luxgarage.com",
                PasswordHash = "", 
                FirstName = "Adam",
                LastName = "Administrator",
                Role = UserRole.Admin,
                WorkplaceId = krakowBranch.Id
            };
            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

            var worker = new Employee
            {
                Email = "worker@luxgarage.com",
                PasswordHash = "",
                FirstName = "Piotr",
                LastName = "Employer",
                Role = UserRole.Employee,
                WorkplaceId = krakowBranch.Id
            };
            worker.PasswordHash = hasher.HashPassword(worker, "worker123");

            var customer1 = new Customer
            {
                Email = "jan.kowalski@gmail.com",
                PasswordHash = "",
                FirstName = "Jan",
                LastName = "Kowalski",
                Role = UserRole.Customer,
                PhoneNumber = "123456789",
                LicenseNumber = "ABC12345"
            };
            customer1.PasswordHash = hasher.HashPassword(customer1, "klient123");

            dbContext.Users.AddRange(admin, worker, customer1);
            dbContext.SaveChanges();
        }

        if (!dbContext.Vehicles.Any())
        {
            dbContext.Vehicles.AddRange(
                new Vehicle
                {
                    Brand = "BMW",
                    Model = "M5 Competition",
                    Horsepower = 625,
                    LicensePlate = "WA 12345",
                    Mileage = 15000,
                    Year = 2023,
                    maxSpeed = 289,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Sedan,
                    Color = VehicleColor.Black,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Brand = "Audi",
                    Model = "RS6 Avant",
                    Horsepower = 600,
                    LicensePlate = "KR 54321",
                    Mileage = 25000,
                    Year = 2022,
                    maxSpeed = 311,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Wagon,
                    Color = VehicleColor.Grey,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Brand = "Porsche",
                    Model = "911 Carrera S",
                    Horsepower = 450,
                    LicensePlate = "GD 99999",
                    Mileage = 5000,
                    Year = 2024,
                    maxSpeed = 352,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Coupe,
                    Color = VehicleColor.Yellow,
                    Status = VehicleStatus.Maintenance
                }
            );
            dbContext.SaveChanges();
        }

        if (!dbContext.Offers.Any())
        {
            var bmw = dbContext.Vehicles.First(v => v.Brand == "BMW");
            var audi = dbContext.Vehicles.First(v => v.Brand == "Audi");

            var offerBmw = new Offer
            {
                VehicleId = bmw.Id,
                Title = "Beast from Monachium - BMW M5",
                Description = "Some descriptionSome descriptionSome descriptionSome description",
                IsActive = true,
                Prices = new List<OfferPrice>
                {
                    new OfferPrice { PricePerDay = 1500.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) }
                }
            };

            var offerAudi = new Offer
            {
                VehicleId = audi.Id,
                Title = "Wow a car - Audi RS6",
                Description = "Some descriptionSome descriptionSome descriptionSome description",
                IsActive = true,
                Prices = new List<OfferPrice>
                {
                    new OfferPrice { PricePerDay = 1200.00m, ValidFrom = DateTime.UtcNow.AddMonths(-2) }
                }
            };

            dbContext.Offers.AddRange(offerBmw, offerAudi);
            dbContext.SaveChanges();
        }

        if (!dbContext.Insurances.Any())
        {
            dbContext.Insurances.AddRange(
                new Insurance { Name = "Full (OC/AC/NNW)", PricePerDay = 150.00m },
                new Insurance { Name = "Tire and glass protection", PricePerDay = 45.00m },
                new Insurance { Name = "No income", PricePerDay = 200.00m }
            );
            dbContext.SaveChanges();
        }
    }

    /// <summary>
    /// Adds the RentalContext to the service collection, 
    /// configuring it to use a PostgreSQL database with the connection string specified in the configuration.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    public static void AddStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<RentalContext>((options) =>
        {
           options.UseNpgsql(connectionString); 
        });
    }
}