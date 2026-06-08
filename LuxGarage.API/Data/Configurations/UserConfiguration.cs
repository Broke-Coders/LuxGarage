using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LuxGarage.API.Features.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).HasMaxLength(150);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(50);
        builder.Property(u => u.LastName).HasMaxLength(50);

        builder.HasDiscriminator(u => u.Role)
            .HasValue<Customer>(UserRole.Customer)
            .HasValue<Employee>(UserRole.Employee)
            .HasValue<User>(UserRole.Admin);
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasOne(e => e.Workplace)
            .WithMany(b => b.Employees)
            .HasForeignKey(e => e.WorkplaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.LicenseNumber).IsRequired();
        builder.Property(c => c.PhoneNumber).IsRequired();
    }
}