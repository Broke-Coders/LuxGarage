using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the VehicleImage entity, defining the database schema and relationships.
/// </summary>
public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
{
    /// <summary>
    /// Configures the VehicleImage entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<VehicleImage> builder)
    {
        builder.Property(vi => vi.StorageKey).HasMaxLength(255);
        builder.Property(vi => vi.OriginalFileName).HasMaxLength(255);
        builder.Property(vi => vi.ContentType).HasMaxLength(50);

        builder.HasOne(vi => vi.Vehicle)
            .WithMany(v => v.Images)
            .HasForeignKey(vi => vi.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}