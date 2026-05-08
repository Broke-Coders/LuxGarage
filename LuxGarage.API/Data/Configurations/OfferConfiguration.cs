using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Description)
            .IsRequired(false);

        builder.Property(o => o.PublicationDate)
            .IsRequired();

        builder.HasOne(o => o.Vehicle)
            .WithMany()
            .HasForeignKey(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Prices)
            .WithOne(vp => vp.Offer)
            .HasForeignKey(vp => vp.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Status)
            .WithMany()
            .HasForeignKey(o => o.VehicleStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}