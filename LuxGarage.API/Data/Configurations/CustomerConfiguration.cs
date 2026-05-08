using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

/// <summary>
/// Configuration for the Customer entity, defining the database schema and relationships.
/// </summary>
/// <remarks>
/// This configuration ensures that the Customer entity is properly mapped to the database, including
/// primary key, unique constraints, default values, and relationships with the Rental entity.
/// </remarks>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Configures the Customer entity's properties and relationships.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
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
