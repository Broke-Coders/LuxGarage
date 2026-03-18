using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Configurations;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Workers");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd();

        builder.Property(w => w.Password)
            .IsRequired();

        builder.HasOne(w => w.Workplace)
            .WithMany(wp => wp.Workers)
            .HasForeignKey(w => w.WorkplaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Permission)
            .WithMany(p => p.Workers)
            .HasForeignKey(w => w.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
