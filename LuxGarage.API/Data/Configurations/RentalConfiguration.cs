using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the Rental entity, defining the database schema and relationships.
/// </summary>
public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    /// <summary>
    /// Configures the Rental entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.Property(r => r.VehiclePriceAtBooking).HasColumnType("decimal(10,2)");
        builder.Property(r => r.TotalPrice).HasColumnType("decimal(12,2)");

        builder.HasOne(r => r.Vehicle)
            .WithMany(v => v.Rentals)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Rentals)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
