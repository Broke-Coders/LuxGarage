using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;
/// <summary>
/// Configuration for the RentalInsurance entity, defining the database schema and relationships.
/// </summary>
public class RentalInsuranceConfiguration : IEntityTypeConfiguration<RentalInsurance>
{
    /// <summary>
    /// Configures the RentalInsurance entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<RentalInsurance> builder)
    {

        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(ri => ri.Rental)
            .WithMany(r => r.RentalInsurances)
            .HasForeignKey(ri => ri.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ri => ri.Insurance)
            .WithMany(i => i.RentalInsurances)
            .HasForeignKey(ri => ri.InsuranceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
