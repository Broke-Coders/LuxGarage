using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Configuration for the Vehicle entity, defining the database schema and relationships.
/// </summary>
public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    /// <summary>
    /// Configures the Vehicle entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Property(v => v.LicensePlate)
            .IsRequired();

        builder.Property(v => v.Horsepower)
            .IsRequired();

        builder.Property(v => v.Mileage)
            .IsRequired();

        builder.HasOne(v => v.VehicleBrand)
            .WithMany(b => b.Vehicles)
            .HasForeignKey(v => v.VehicleBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleModel)
            .WithMany(m => m.Vehicles)
            .HasForeignKey(v => v.VehicleModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleBody)
            .WithMany(b => b.Vehicles)
            .HasForeignKey(v => v.VehicleBodyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleColor)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.VehicleColorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Images)
            .WithOne(vi => vi.Vehicle)
            .HasForeignKey(vi => vi.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
