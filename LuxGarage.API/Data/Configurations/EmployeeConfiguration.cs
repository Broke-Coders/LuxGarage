using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Workers");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd();

        builder.Property(w => w.Password)
            .IsRequired();

        builder.HasOne(w => w.Workplace)
            .WithMany(wp => wp.Employees)
            .HasForeignKey(w => w.WorkplaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Permission)
            .WithMany(p => p.Employees)
            .HasForeignKey(w => w.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
