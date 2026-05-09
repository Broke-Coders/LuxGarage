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
        builder.ToTable("VehicleImages");

        builder.HasKey(vi => vi.Id);

        builder.Property(vi => vi.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vi => vi.StorageKey)
            .IsRequired();

        builder.HasIndex(vi => vi.StorageKey)
            .IsUnique();

        builder.Property(vi => vi.OriginalFileName)
            .IsRequired();

        builder.Property(vi => vi.ContentType)
            .IsRequired();

        builder.Property(vi => vi.FileSize)
            .IsRequired();

        builder.Property(vi => vi.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(vi => vi.Vehicle)
            .WithMany()
            .HasForeignKey(vi => vi.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}