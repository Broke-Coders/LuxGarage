using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class VehicleBrandConfiguration : IEntityTypeConfiguration<VehicleBrand>
{
    public void Configure(EntityTypeBuilder<VehicleBrand> builder)
    {
        builder.HasKey(vb => vb.Id);

        builder.Property(vb => vb.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vb => vb.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(vb => vb.Vehicles)
            .WithOne(v => v.VehicleBrand)
            .HasForeignKey(v => v.VehicleBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
