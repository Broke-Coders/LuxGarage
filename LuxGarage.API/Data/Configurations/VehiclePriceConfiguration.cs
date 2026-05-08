using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the VehiclePrice entity, defining the database schema and relationships.
/// </summary>
public class VehiclePriceConfiguration : IEntityTypeConfiguration<VehiclePrice>
{
    /// <summary>
    /// Configures the VehiclePrice entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<VehiclePrice> builder)
    {
        builder.HasKey(vp => vp.Id);

        builder.Property(vp => vp.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vp => vp.ValidFrom)
            .IsRequired();

        builder.Property(vp => vp.ValidTo)
            .IsRequired(false);
    
        builder.Property(vp => vp.PricePerDay)
            .IsRequired();
        
        builder.HasOne(vp => vp.Offer)
            .WithMany(o => o.Prices)
            .HasForeignKey(vp => vp.OfferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}