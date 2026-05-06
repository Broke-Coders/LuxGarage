using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;

namespace LuxGarage.API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<RentalContext>();


        //dbContext.Database.EnsureDeleted(); // Reseting database
        //dbContext.Database.CloseConnection();
        dbContext.Database.Migrate();

    if (!dbContext.Set<Permission>().Any())
    {
        dbContext.Set<Permission>().AddRange(
        new Permission { Name = "Admin" },
        new Permission { Name = "Worker" }
        );
        dbContext.SaveChanges();
    }


        if(!dbContext.Set<Workplace>().Any())
        {
            dbContext.Set<Workplace>().AddRange(
                new Workplace {Country = "Poland", 
                City = "Krakow", Street = "Kulszowa", 
                BuildingNumber = 1},

                new Workplace {Country = "USA", City = "New York", 
                Street = "Chinastreet", BuildingNumber = 3},

                new Workplace {Country = "Germany", City = "Berlin", 
                Street = "Hausstradt", BuildingNumber = 12 }
            );
            dbContext.SaveChanges();
        }

        if(!dbContext.Set<Employee>().Any())
        {
            int permId = dbContext.Set<Permission>().First(p => p.Name == "Admin").Id;
            int WorkId = dbContext.Set<Workplace>().First(p => p.City == "Krakow").Id;

           dbContext.Set<Employee>().AddRange( 
                new Employee {Password = "pass", Login = "login1",
                WorkplaceId = WorkId, PermissionId = permId},

                new Employee {Password = "haslo", Login = "login555",
                WorkplaceId = WorkId, PermissionId = permId},

                new Employee {Password = "1234", Login = "login67",
                WorkplaceId = WorkId, PermissionId = permId}
           );
           dbContext.SaveChanges();
        }

        if(!dbContext.Set<VehicleBrand>().Any())
        {
            dbContext.Set<VehicleBrand>().AddRange(
                new VehicleBrand {Name = "BMW"},
                new VehicleBrand {Name = "Audi"},
                new VehicleBrand {Name = "Volkswagen"}
            );
            dbContext.SaveChanges();
        }

        if (!dbContext.Set<VehicleModel>().Any())
        {
            dbContext.Set<VehicleModel>().AddRange(
                new VehicleModel
                {
                    Name = "M5 Competition",
                    VehicleBrandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "BMW").Id
                },
                new VehicleModel
                {
                    Name = "RS5",
                    VehicleBrandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "Audi").Id
                },
                new VehicleModel
                {
                    Name = "Golf R",
                    VehicleBrandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "Volkswagen").Id
                }
            );
            dbContext.SaveChanges();
        }

        if (!dbContext.Set<VehicleBody>().Any())
        {
            dbContext.Set<VehicleBody>().AddRange(
                new VehicleBody {Name = "Sedan"},
                new VehicleBody { Name = "Hatchback" },
                new VehicleBody { Name = "Coupe" },
                new VehicleBody { Name = "Wagon" },
                new VehicleBody { Name = "Cabriolet" },
                new VehicleBody { Name = "Roadster" },
                new VehicleBody { Name = "Pickup" },
                new VehicleBody {Name = "Van"},
                new VehicleBody {Name = "Pickup"}
            );
            dbContext.SaveChanges();
        }

        if(!dbContext.Set<VehicleColor>().Any())
        {
            dbContext.Set<VehicleColor>().AddRange(
                new VehicleColor{Name = "WHITE", 
                HtmlColor = "#FFFFFF"},
                new VehicleColor{Name = "BLACK", 
                HtmlColor = "#000000"},
                new VehicleColor{Name = "RED", 
                HtmlColor = "#F00000"}
            );
            dbContext.SaveChanges();
        }

        if (!dbContext.Set<Vehicle>().Any())
        {

            int brandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "BMW").Id;
            int modelId = dbContext.Set<VehicleModel>().First(m => m.Name == "M5 Competition").Id;
            int bodyId = dbContext.Set<VehicleBody>().First(b => b.Name == "Sedan").Id;
            int colorId = dbContext.Set<VehicleColor>().First(c => c.Name == "WHITE").Id;
            dbContext.Set<Vehicle>().AddRange(
                new Vehicle
                {
                    VehicleBrandId = brandId,
                    VehicleModelId = modelId,
                    Horsepower = 440,
                    LicensePlate = "SBI 12345",
                    Mileage = 50000,
                    VehicleBodyId = bodyId,
                    VehicleColorId = colorId,
                },
                new Vehicle
                {
                    VehicleBrandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "Audi").Id,
                    VehicleModelId = dbContext.Set<VehicleModel>().First(m => m.Name == "RS5").Id,
                    Horsepower = 500,
                    LicensePlate = "WPR 12345",
                    Mileage = 30000,
                    VehicleBodyId = dbContext.Set<VehicleBody>().First(b => b.Name == "Sedan").Id,
                    VehicleColorId = dbContext.Set<VehicleColor>().First(c => c.Name == "BLACK").Id
                },
                new Vehicle
                {
                    VehicleBrandId = dbContext.Set<VehicleBrand>().First(b => b.Name == "Volkswagen").Id,
                    VehicleModelId = dbContext.Set<VehicleModel>().First(m => m.Name == "Golf R").Id,
                    Horsepower = 280,
                    LicensePlate = "SZY 12345",
                    Mileage = 44444,
                    VehicleBodyId = dbContext.Set<VehicleBody>().First(b => b.Name == "Hatchback").Id,
                    VehicleColorId = dbContext.Set<VehicleColor>().First(c => c.Name == "RED").Id
                }
                );
        }

        if(!dbContext.Set<Insurance>().Any())
        {
            dbContext.Set<Insurance>().AddRange(
                new Insurance {Name = "anti-thief",
                Price = 10.99m},
                new Insurance {Name = "anti-scratch",
                Price = 18.99m},
                new Insurance {Name = "tire-insurance",
                Price = 6.59m}
            );
            dbContext.SaveChanges();
        }

        if(!dbContext.Set<Customer>().Any())
        {
            dbContext.Set<Customer>().AddRange(
                new Customer{Email = "wuj@gmail.com",
                BorrowCounter = 1},
                new Customer{Email = "ziutek@outlook.com",
                BorrowCounter = 5},
                new Customer{Email = "hop@maupa.net",
                BorrowCounter = 3}
            );
            dbContext.SaveChanges();
        }
    }

    public static void AddStoreDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<RentalContext>((sp, options) =>
        {
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); 
        });
    }
}