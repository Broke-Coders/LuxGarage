using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;
using Microsoft.AspNetCore.Identity;
using LuxGarage.API.Features.Users;
using LuxGarage.API.Features.Vehicles;
using LuxGarage.API.Data;

namespace LuxGarage.API.Extensions;
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
                /////////////////////////////////////////////////////////
                
                new Vehicle
                {
                    Brand = "Ferrari",
                    Model = "F8 Spider",
                    Horsepower = 710,
                    LicensePlate = "F8 SPIDER",
                    EngineName = "3.9L V8 Twin-Turbo",
                    Mileage = 1200,
                    Year = 2023,
                    SpeedToHundred = 2.9f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Cabriolet,
                    Color = VehicleColor.Red,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Lamborghini",
                    Model = "Huracan Evo Spyder",
                    Horsepower = 640,
                    LicensePlate = "EVO SPYDR",
                    EngineName = "5.2L V10",
                    Mileage = 2500,
                    Year = 2022,
                    SpeedToHundred = 3.1f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Cabriolet,
                    Color = VehicleColor.Grey,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Ferrari",
                    Model = "488 Spider",
                    Horsepower = 661,
                    LicensePlate = "488 SPIDR",
                    EngineName = "3.9L V8 Twin-Turbo",
                    Mileage = 8500,
                    Year = 2019,
                    SpeedToHundred = 3.0f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Cabriolet,
                    Color = VehicleColor.Red,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Lamborghini",
                    Model = "Urus SE",
                    Horsepower = 789,
                    LicensePlate = "URUS SE",
                    EngineName = "4.0L V8 Twin-Turbo Hybrid",
                    Mileage = 500,
                    Year = 2024,
                    SpeedToHundred = 3.4f,
                    EngineType = EngineType.Hybrid,
                    BodyType = VehicleBodyType.SUV,
                    Color = VehicleColor.Black,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Lamborghini",
                    Model = "Huracan Spyder",
                    Horsepower = 602,
                    LicensePlate = "HURACAN S",
                    EngineName = "5.2L V10",
                    Mileage = 12000,
                    Year = 2018,
                    SpeedToHundred = 3.4f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Cabriolet,
                    Color = VehicleColor.White,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },

                /////////////////////////////////////////////////////////
                new Vehicle
                {
                    Brand = "BMW",
                    Model = "M5 Competition",
                    Horsepower = 625,
                    LicensePlate = "WA 12345",
                    EngineName = "4.4 V8",
                    Mileage = 15000,
                    Year = 2023,
                    SpeedToHundred = 3.5f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Sedan,
                    Color = VehicleColor.Black,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Audi",
                    Model = "RS6 Avant",
                    Horsepower = 600,
                    LicensePlate = "KR 54321",
                    EngineName = "V8",
                    Mileage = 25000,
                    Year = 2022,
                    SpeedToHundred = 2.7f,
                    EngineType = EngineType.Gasoline,
                    BodyType = VehicleBodyType.Wagon,
                    Color = VehicleColor.Grey,
                    Status = VehicleStatus.Available,
                    Images = new List<VehicleImage>
                    {
                        new VehicleImage { StorageKey = "1.webp", OriginalFileName = "1.webp", ContentType = "image/webp", IsPrimary = true }
                    }
                },
                new Vehicle
                {
                    Brand = "Porsche",
                    Model = "911 Carrera S",
                    Horsepower = 450,
                    LicensePlate = "GD 99999",
                    EngineName = "3.0 flat six",
                    Mileage = 5000,
                    Year = 2024,
                    SpeedToHundred = 3.3f,
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
            var f8 = dbContext.Vehicles.First(v => v.Model == "F8 Spider");
            var evo = dbContext.Vehicles.First(v => v.Model == "Huracan Evo Spyder");
            var f488 = dbContext.Vehicles.First(v => v.Model == "488 Spider");
            var urus = dbContext.Vehicles.First(v => v.Model == "Urus SE");
            var huracan = dbContext.Vehicles.First(v => v.Model == "Huracan Spyder");
            var bmw = dbContext.Vehicles.First(v => v.Brand == "BMW");
            var audi = dbContext.Vehicles.First(v => v.Brand == "Audi");

            var offers = new List<Offer>
            {
                new Offer
                {
                    VehicleId = f8.Id,
                    Title = "Italian Masterpiece - Ferrari F8 Spider",
                    Description = "The Ferrari F8 Spider delivers an incredible open-top driving experience paired with unmistakable Italian design. Powered by a twin-turbocharged 3.9L V8 engine, it produces exhilarating performance with a lightning-fast 7-speed dual-clutch transmission and rear-wheel drive. The retractable hard top opens in seconds, allowing you to enjoy the sound of the V8 at any speed. Finished with a sharp, aggressive exterior and a driver-focused interior, the F8 Spider offers the perfect balance of supercar performance, luxury, and everyday usability.",
                    IsActive = true,
                    Prices = new List<OfferPrice> { new OfferPrice { PricePerDay = 3500.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) } }
                },
                new Offer
                {
                    VehicleId = evo.Id,
                    Title = "V10 Symphony - Lamborghini Huracan Evo Spyder",
                    Description = "The Lamborghini Huracan Evo Spyder delivers an incredible open-top driving experience paired with unmistakable Sant'Agata design. Powered by a naturally aspirated 5.2L V10 engine, it produces exhilarating performance with a lightning-fast dual-clutch transmission and all-wheel drive. The power-operated soft top opens in seconds, allowing you to enjoy the sound of the V10 at any speed. Finished with a sharp, aggressive exterior and a driver-focused interior, the Evo Spyder offers the perfect balance of supercar performance, luxury, and everyday usability.",
                    IsActive = true,
                    Prices = new List<OfferPrice> { new OfferPrice { PricePerDay = 3800.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) } }
                },
                new Offer
                {
                    VehicleId = f488.Id,
                    Title = "Turbocharged Legend - Ferrari 488 Spider",
                    Description = "The Ferrari 488 Spider delivers an incredible open-top driving experience paired with unmistakable Italian design. Powered by a twin-turbocharged 3.9L V8 engine, it produces exhilarating performance with a lightning-fast 7-speed dual-clutch transmission and rear-wheel drive. The retractable hard top opens in seconds, allowing you to enjoy the sound of the V8 at any speed. Finished with a sharp, aggressive exterior and a driver-focused interior, the 488 Spider offers the perfect balance of supercar performance, luxury, and everyday usability.",
                    IsActive = true,
                    Prices = new List<OfferPrice> { new OfferPrice { PricePerDay = 3000.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) } }
                },
                new Offer
                {
                    VehicleId = urus.Id,
                    Title = "The Ultimate SUV - Lamborghini Urus SE",
                    Description = "The Lamborghini Urus SE delivers an incredible performance SUV experience paired with unmistakable Lamborghini design. Powered by a twin-turbocharged 4.0L V8 hybrid engine, it produces exhilarating performance with a lightning-fast 8-speed automatic transmission and all-wheel drive. The advanced hybrid system provides instant torque, allowing you to enjoy the power at any speed. Finished with a sharp, aggressive exterior and a driver-focused interior, the Urus SE offers the perfect balance of supercar performance, luxury, and everyday usability.",
                    IsActive = true,
                    Prices = new List<OfferPrice> { new OfferPrice { PricePerDay = 4000.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) } }
                },
                new Offer
                {
                    VehicleId = huracan.Id,
                    Title = "Pure Emotion - Lamborghini Huracan Spyder",
                    Description = "The Lamborghini Huracan Spyder delivers an incredible open-top driving experience paired with unmistakable Italian design. Powered by a naturally aspirated 5.2L V10 engine, it produces exhilarating performance with a lightning-fast dual-clutch transmission and all-wheel drive. The power-operated soft top opens in seconds, allowing you to enjoy the sound of the V10 at any speed. Finished with a sharp, aggressive exterior and a driver-focused interior, the Huracan Spyder offers the perfect balance of supercar performance, luxury, and everyday usability.",
                    IsActive = true,
                    Prices = new List<OfferPrice> { new OfferPrice { PricePerDay = 3200.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) } }
                },
                new Offer
                {
                    VehicleId = bmw.Id,
                    Title = "Beast from Monachium - BMW M5",
                    Description = "Some descriptionSome descriptionSome descriptionSome description",
                    IsActive = true,
                    Prices = new List<OfferPrice>
                    {
                        new OfferPrice { PricePerDay = 1500.00m, ValidFrom = DateTime.UtcNow.AddMonths(-1) }
                    }
                },
                new Offer
                {
                    VehicleId = audi.Id,
                    Title = "Wow a car - Audi RS6",
                    Description = "Some descriptionSome descriptionSome descriptionSome description",
                    IsActive = true,
                    Prices = new List<OfferPrice>
                    {
                        new OfferPrice { PricePerDay = 1200.00m, ValidFrom = DateTime.UtcNow.AddMonths(-2) }
                    }
                }
            };

            dbContext.Offers.AddRange(offers);
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