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
        builder.Property(v => v.LicensePlate).HasMaxLength(20);
        builder.HasIndex(v => v.LicensePlate).IsUnique();

        builder.Property(v => v.Brand).HasMaxLength(50);
        builder.Property(v => v.Model).HasMaxLength(50);
        builder.Property(v => v.Horsepower).HasColumnType("decimal(6,2)");
    }
}
