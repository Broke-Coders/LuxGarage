using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class WorkplaceConfiguration : IEntityTypeConfiguration<Workplace>
{
    public void Configure(EntityTypeBuilder<Workplace> builder)
    {
        builder.ToTable("Workplaces");

        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Id)
            .ValueGeneratedOnAdd();

        builder.Property(wp => wp.Country)
            .IsRequired();

        builder.Property(wp => wp.City)
            .IsRequired();

        builder.Property(wp => wp.Street)
            .IsRequired();

        builder.Property(wp => wp.BuildingNumber)
            .IsRequired();

        builder.HasMany(wp => wp.Employees)
            .WithOne(w => w.Workplace)
            .HasForeignKey(w => w.WorkplaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
