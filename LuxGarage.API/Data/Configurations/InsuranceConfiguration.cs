using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
{
    public void Configure(EntityTypeBuilder<Insurance> builder)
    {

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.Name)
            .IsRequired();

        builder.Property(i => i.Price)
            .IsRequired();

        builder.HasMany(i => i.RentalInsurances)
            .WithOne(ri => ri.Insurance)
            .HasForeignKey(ri => ri.InsuranceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
