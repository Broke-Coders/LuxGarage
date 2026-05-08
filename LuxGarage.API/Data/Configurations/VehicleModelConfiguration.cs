using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxGarage.API.Data.Configurations
{
    /// <summary>
    /// Configuration for the VehicleModel entity, defining the database schema and relationships.
    /// </summary>
    public class VehicleModelConfiguration : IEntityTypeConfiguration<VehicleModel>
    {
        /// <summary>
        /// Configures the VehicleModel entity's properties and relationships.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
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
