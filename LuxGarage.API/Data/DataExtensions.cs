using System;
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


        dbContext.Database.EnsureDeleted(); // Reseting database

        dbContext.Database.Migrate();

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

        if(!dbContext.Set<Worker>().Any())
        {
           dbContext.Set<Worker>().AddRange( 
                new Worker {Password = "pass", 
                WorkplaceId = 1, PermissionId = 1},

                new Worker {Password = "haslo", 
                WorkplaceId = 2, PermissionId = 2},

                new Worker {Password = "1234", 
                WorkplaceId = 2, PermissionId = 3}
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

        if(!dbContext.Set<VehicleBody>().Any())
        {
            dbContext.Set<VehicleBody>().AddRange(
                new VehicleBody {Name = "Sedan"},
                new VehicleBody {Name = "Van"},
                new VehicleBody {Name = "Pickup"}
            );
            dbContext.SaveChanges();
        }

        if(!dbContext.Set<VehicleColor>().Any())
        {
            dbContext.Set<VehicleColor>().AddRange(
                new VehicleColor{Name = "WHITE", 
                HtmlColor = "#FFF"},
                new VehicleColor{Name = "BLACK", 
                HtmlColor = "#000"},
                new VehicleColor{Name = "RED", 
                HtmlColor = "#F00"}
            );
            dbContext.SaveChanges();
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

        if(!dbContext.Set<Borrower>().Any())
        {
            dbContext.Set<Borrower>().AddRange(
                new Borrower{Email = "wuj@gmail.com",
                BorrowCounter = 1},
                new Borrower{Email = "ziutek@outlook.com",
                BorrowCounter = 5},
                new Borrower{Email = "hop@maupa.net",
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