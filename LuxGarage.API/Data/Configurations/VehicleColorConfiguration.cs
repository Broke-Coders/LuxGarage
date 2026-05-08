using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the VehicleColor entity, defining the database schema and relationships.
/// </summary>
public class VehicleColorConfiguration : IEntityTypeConfiguration<VehicleColor>
{
    /// <summary>
    /// Configures the VehicleColor entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<VehicleColor> builder)
    {
        builder.ToTable("VehicleColors");

        builder.HasKey(vc => vc.Id);

        builder.Property(vc => vc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vc => vc.Name)
            .IsRequired();

        builder.Property(vc => vc.HtmlColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasIndex(vc => vc.HtmlColor)
            .IsUnique();

        builder.HasMany(vc => vc.Vehicles)
            .WithOne(v => v.VehicleColor)
            .HasForeignKey(v => v.VehicleColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
