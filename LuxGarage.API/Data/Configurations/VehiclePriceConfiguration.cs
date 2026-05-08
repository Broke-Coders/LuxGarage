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