using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Property(v => v.LicensePlate)
            .IsRequired();

        builder.Property(v => v.Horsepower)
            .IsRequired();

        builder.Property(v => v.Mileage)
            .IsRequired();

        builder.HasOne(v => v.VehicleBrand)
            .WithMany(b => b.Vehicles)
            .HasForeignKey(v => v.VehicleBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleBody)
            .WithMany(b => b.Vehicles)
            .HasForeignKey(v => v.VehicleBodyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VehicleColor)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.VehicleColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
