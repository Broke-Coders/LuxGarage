using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the VehicleBody entity, defining the database schema and relationships.
/// </summary>
public class VehicleBodyConfiguration : IEntityTypeConfiguration<VehicleBody>
{
    /// <summary>
    /// Configures the VehicleBody entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<VehicleBody> builder)
    {
        builder.HasKey(vb => vb.Id);

        builder.Property(vb => vb.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vb => vb.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(vb => vb.Vehicles)
            .WithOne(v => v.VehicleBody)
            .HasForeignKey(v => v.VehicleBodyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
