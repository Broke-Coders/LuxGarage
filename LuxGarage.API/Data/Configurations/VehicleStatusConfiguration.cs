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
        // Primary Key
        builder.HasKey(s => s.Id);

        // Name property - enum conversion to string
        builder.Property(s => s.Name)
               .HasConversion<string>()
               .IsRequired();

        // Description property
        builder.Property(s => s.Description)
               .HasMaxLength(500)
               .IsRequired()
               .HasDefaultValue("UNKNOWN");

        // IsAvailable property
        builder.Property(s => s.IsAvailable)
               .IsRequired()
               .HasDefaultValue(false);

        // StartingDate property
        builder.Property(s => s.StartingDate)
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // DateToEnd property - optional
        builder.Property(s => s.DateToEnd)
               .IsRequired(false);
    }
}