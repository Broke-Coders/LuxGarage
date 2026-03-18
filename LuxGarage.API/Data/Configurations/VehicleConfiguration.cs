using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LuxGarage.API.Models;

namespace LuxGarage.API.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Property(v => v.Horsepower)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(v => v.Mileage)
            .IsRequired();

        builder.HasOne(v => v.VehicleBrand)
            .WithMany()
            .HasForeignKey(v => v.VehicleBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleBody)
            .WithMany()
            .HasForeignKey(v => v.VehicleBodyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleColor)
            .WithMany()
            .HasForeignKey(v => v.VehicleColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}