using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class VehiclePriceConfiguration : IEntityTypeConfiguration<VehiclePrice>
{
    public void Configure(EntityTypeBuilder<VehiclePrice> builder)
    {
        builder.HasKey(vp => vp.Id);

        builder.Property(vp => vp.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vp => vp.PricePerDay)
            .IsRequired();
            
    }
}