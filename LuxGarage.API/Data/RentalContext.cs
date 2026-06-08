using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using LuxGarage.API.Models;

namespace LuxGarage.API.Data;

/// <summary>
/// The RentalContext class represents the Entity Framework Core database context for the LuxGarage application,
/// providing access to the database and defining the DbSet properties for each entity in the application.
/// </summary>
/// <param name="options">options for configuring the database context.</param>
public class RentalContext(DbContextOptions<RentalContext> options)
: DbContext(options)
{
    public DbSet<Insurance> Insurances => Set<Insurance>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<OfferPrice> OfferPrices => Set<OfferPrice>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalInsurance> RentalInsurances => Set<RentalInsurance>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
    public DbSet<Workplace> Workplaces => Set<Workplace>();

    /// <summary>
    /// Configures the entity mappings and relationships for the database context using the Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RentalContext).Assembly);
    }
}

public class RentalContextFactory : IDesignTimeDbContextFactory<RentalContext>
{
    public RentalContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RentalContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new RentalContext(optionsBuilder.Options);
    }
}