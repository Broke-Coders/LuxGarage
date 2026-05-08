using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;
/// <summary>
/// Configuration for the Employee entity, defining the database schema and relationships.
/// </summary>
/// <remarks>
/// This configuration ensures that the Employee entity is properly mapped to the database, including
/// primary key, unique constraints, default values, and relationships with the Workplace and Permission entities.
/// </remarks>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
        /// <summary>
        /// Configures the Employee entity's properties and relationships.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Employee> builder)
    {

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
