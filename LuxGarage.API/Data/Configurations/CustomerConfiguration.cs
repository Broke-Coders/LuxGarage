using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Email)
            .IsRequired();

        builder.HasIndex(b => b.Email)
            .IsUnique();

        builder.Property(b => b.BorrowCounter)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasMany(b => b.Rentals)
            .WithOne(r => r.Customer)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
