using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations
{
    public class VehicleModelRepository : IVehicleModelRepository
    {
        private readonly RentalContext _context;

        public VehicleModelRepository(RentalContext context)
        {
            _context = context;
        }

        public async Task<VehicleModel?> GetByIdAsync(int id)
            => await _context.VehicleModels.FindAsync(id);

        public async Task AddAsync(VehicleModel model)
        {
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleModel model, int id)
        {
            VehicleModel? oldModel = await _context.VehicleModels.FindAsync(id);

            if (oldModel != null)
            {
                oldModel.Name = model.Name;
                oldModel.VehicleBrandId = model.VehicleBrandId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);

            if (model != null)
            {
                Console.WriteLine("Model brand with given id not found");
                return;
            }

            _context.VehicleModels.Remove(model);
            await _context.SaveChangesAsync();
        }

    }
}
