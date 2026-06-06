using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Configuration for the Vehicle status entity, defining the database schema and relationships.
/// </summary>
public class VehicleStatusConfiguration : IEntityTypeConfiguration<VehicleStatus>
{
    public void Configure(EntityTypeBuilder<VehicleStatus> builder)
    {
        builder.Property(s => s.Name)
               .HasConversion<string>();
    }
}