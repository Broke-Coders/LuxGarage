using LuxGarage.API.Data;
using LuxGarage.API.Models;
using LuxGarage.API.Repositories.Interfaces;

namespace LuxGarage.API.Repositories.Implementations
{
    /// <summary>
    /// Represents a repository for managing vehicle model data in the LuxGarage API,
    /// providing methods for retrieving, adding, updating, and deleting vehicle model information from the database
    /// </summary>
    public class VehicleModelRepository : IVehicleModelRepository
    {
        private readonly RentalContext _context;

        /// <summary>
        /// Initializes a new instance of the VehicleModelRepository class, providing the necessary context for accessing the database
        /// and performing operations on vehicle model data.
        /// </summary>
        /// <param name="context">The database context for accessing vehicle model data.</param>
        public VehicleModelRepository(RentalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a vehicle model by its id from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the vehicle model to retrieve.</param>
        /// <returns>The vehicle model with the specified identifier, or null if not found.</returns>
        public async Task<VehicleModel?> GetByIdAsync(int id)
            => await _context.VehicleModels.FindAsync(id);

        /// <summary>
        /// Adds a new vehicle model to the database.
        /// </summary>
        /// <param name="model">The vehicle model to add.</param>
        public async Task AddAsync(VehicleModel model)
        {
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing vehicle model in the database.
        /// </summary>
        /// <param name="model">The updated vehicle model information.</param>
        /// <param name="id">The unique identifier of the vehicle model to update.</param>
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

        /// <summary>
        /// Deletes a vehicle model from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the vehicle model to delete.</param>
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
