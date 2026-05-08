using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
{
    public void Configure(EntityTypeBuilder<VehicleImage> builder)
    {
        builder.ToTable("VehicleImages");

        builder.HasKey(vi => vi.Id);

        builder.Property(vi => vi.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vi => vi.StorageKey)
            .IsRequired();

        builder.HasIndex(vi => vi.StorageKey)
            .IsUnique();

        builder.Property(vi => vi.OriginalFileName)
            .IsRequired();

        builder.Property(vi => vi.ContentType)
            .IsRequired();

        builder.Property(vi => vi.FileSize)
            .IsRequired();

        builder.Property(vi => vi.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(vi => vi.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(vi => vi.Vehicle)
            .WithMany()
            .HasForeignKey(vi => vi.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}