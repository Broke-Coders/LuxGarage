using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;
/// <summary>
/// Configuration for the Offer entity, defining the database schema and relationships.
/// </summary>
public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    /// <summary>
    /// Configures the Offer entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.Property(o => o.Title).HasMaxLength(150);

        builder.HasOne(o => o.Vehicle)
            .WithOne()
            .HasForeignKey<Offer>(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}