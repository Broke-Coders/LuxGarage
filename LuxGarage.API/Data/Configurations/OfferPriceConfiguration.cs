using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Features.Offers;

public class OfferPriceConfiguration : IEntityTypeConfiguration<OfferPrice>
{
    public void Configure(EntityTypeBuilder<OfferPrice> builder)
    {
        builder.Property(op => op.PricePerDay).HasColumnType("decimal(10,2)");

        builder.HasOne(op => op.Offer)
            .WithMany(o => o.Prices)
            .HasForeignKey(op => op.OfferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}