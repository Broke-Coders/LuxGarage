using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the Workplace entity, defining the database schema and relationships.
/// </summary>
public class WorkplaceConfiguration : IEntityTypeConfiguration<Workplace>
{
    /// <summary>
    /// Configures the Workplace entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Workplace> builder)
    {
        builder.Property(wp => wp.Country).IsRequired();
        builder.Property(wp => wp.City).IsRequired();
        builder.Property(wp => wp.Street).IsRequired();
        builder.Property(wp => wp.BuildingNumber).IsRequired();
    }
}
