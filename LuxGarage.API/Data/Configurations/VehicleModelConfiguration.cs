using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Data.Configurations
{
    public class VehicleModelConfiguration : IEntityTypeConfiguration<VehicleModel>
    {
        public void Configure(EntityTypeBuilder<VehicleModel> builder)
        {
            builder.HasKey(vm => vm.Id);
            
            builder.Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(m => m.VehicleBrand)
                .WithMany(b => b.VehicleModels)
                .HasForeignKey(m => m.VehicleBrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
