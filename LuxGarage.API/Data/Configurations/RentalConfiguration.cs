using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.StartingTime)
            .IsRequired();

        builder.Property(r => r.AppointedReturnTime)
            .IsRequired();

        builder.Property(r => r.RealReturnTime)
            .IsRequired(false);

        builder.HasOne(r => r.Vehicle)
            .WithMany(v => v.Rentals)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Borrower)
            .WithMany(b => b.Rentals)
            .HasForeignKey(r => r.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.RentalInsurances)
            .WithOne(ri => ri.Rental)
            .HasForeignKey(ri => ri.RentalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
