using System;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;

namespace LuxGarage.API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        //TODO implement
    }

    public static void AddStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<RentalContext>((sp, options) =>
        {
           options.UseNpgsql(connString); 
        });
    }
}