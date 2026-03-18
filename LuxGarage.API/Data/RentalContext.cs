using System;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Models;

namespace LuxGarage.API.Data;

public class RentalContext(DbContextOptions<RentalContext> options)
: DbContext(options)
{
    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Insurance> Insurances => Set<Insurance>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalInsurance> RentalInsurances => Set<RentalInsurance>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleBody> VehicleBodies => Set<VehicleBody>();
    public DbSet<VehicleBrand> VehicleBrands => Set<VehicleBrand>();
    public DbSet<VehicleColor> VehicleColors => Set<VehicleColor>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<Workplace> Workplaces => Set<Workplace>();
}