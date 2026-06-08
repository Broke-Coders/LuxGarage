using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the Insurance entity, defining the database schema and relationships.
/// </summary>
public class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
{
    /// <summary>
    /// Configures the Insurance entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Insurance> builder)
    {
        builder.Property(i => i.Name).HasMaxLength(100);
        builder.Property(i => i.PricePerDay).HasColumnType("decimal(8,2)");
    }
}
