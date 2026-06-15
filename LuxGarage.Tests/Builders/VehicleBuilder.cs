using LuxGarage.API.Data;
using LuxGarage.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxGarage.Tests.Builders;

/// <summary>
/// Builder for creating Vehicle instances with related entities (Brand, Model, Body, Color).
/// </summary>
/// <remarks>
/// This builder simplifies the creation of Vehicle objects for testing by providing default values and allowing customization through fluent methods.
/// It also handles the creation and saving of related entities to ensure that the Vehicle can be properly
/// constructed with valid foreign key references.
/// The BuildAsync method requires a RentalContext to save the related entities and retrieve their IDs for the Vehicle.
/// Example usage:
/// var vehicle = await new VehicleBuilder()
///     .WithLicensePlate("TEST 123")
///     .WithMileage(5000)
///    .BuildAsync(context);
/// </remarks>
/// 
public class VehicleBuilder
{
    // default values for properties and related entities
    private string _licensePlate = "DEFAULT 01";
    private int _mileage = 1000;
    private string _brandName = "Default Brand";
    private string _modelName = "Default Model";
    private string _bodyName = "Default Body";
    private string _colorName = "Default Color";
    private string _colorHtml = "#000000";
    private decimal _horsepower = 100;

    /// <summary>
    /// Fluent method to set the license plate of the vehicle.
    /// </summary>
    /// <param name="licensePlate">The license plate to set.</param>
    /// <returns>The builder instance for chaining.</returns>
    public VehicleBuilder WithLicensePlate(string licensePlate)
    {
        _licensePlate = licensePlate;
        return this;
    }

    /// <summary>
    /// Fluent method to set the mileage of the vehicle.
    /// </summary>
    /// <param name="mileage">The mileage to set.</param>
    /// <returns>The builder instance for chaining.</returns>
    public VehicleBuilder WithMileage(int mileage)
    {
        _mileage = mileage;
        return this;
    }

    /// <summary>
    /// Fluent method to set the brand and model of the vehicle. 
    /// </summary>
    /// <remarks>
    /// This will create new VehicleBrand and VehicleModel entities with the specified names when building the Vehicle. The model will be associated with the brand.
    /// </remarks>
    /// <param name="brandName"> The brand name to set. </param>
    /// <param name="modelName"> The model name to set. </param>
    /// <returns> The builder instance for chaining. </returns>
    public VehicleBuilder WithBrandAndModel(string brandName, string modelName)
    {
        _brandName = brandName;
        _modelName = modelName;
        return this;
    }

    /// <summary>
    /// Fluent method to set the body name of the vehicle. This will create a new VehicleBody entity with the specified name when building the Vehicle.
    /// </summary>
    /// <param name="body">The body name to set.</param>
    /// <returns>The builder instance for chaining.</returns>
    public VehicleBuilder WithBody(string bodyName)
    {
        _bodyName = bodyName;
        return this;
    }

    /// <summary>
    /// Fluent method to set the color name of the vehicle. This will create a new VehicleColor entity with the specified name when building the Vehicle.
    /// </summary>
    /// <param name="colorName">The color name to set.</param>
    /// <param name="colorHtml">The HTML color code to set for the color. This is optional and defaults to black (#000000) if not provided.</param>
    /// <returns>The builder instance for chaining.</returns>
    public VehicleBuilder WithColor(string colorName, string colorHtml = "#000000")
    {
        _colorName = colorName;
        _colorHtml = colorHtml;
        return this;
    }

    /// <summary>
    /// Fluent method to set the horsepower of the vehicle. This will set the Horsepower property of the Vehicle when building it.
    /// </summary>
    /// <param name="horsepower">The horsepower to set.</param>
    /// <returns>The builder instance for chaining.</returns>
    public VehicleBuilder WithHorsepower(decimal horsepower)
    {
        _horsepower = horsepower;
        return this;
    }



    /// <summary>
    /// Builds the Vehicle instance with the specified properties and related entities. 
    /// This method saves the related entities to the database context before creating the Vehicle to ensure that all foreign key references are valid.
    /// </summary>
    /// <param name="context">The rental context to use for saving related entities.</param>
    /// <returns>The built Vehicle instance as Task<Vehicle></returns>
    public async Task<Vehicle> BuildAndSaveAsync(RentalContext context)
    {
        var vehicle = await BuildAsync(context);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        return vehicle;
    }
    
    /// <summary>
    ///  Builds the Vehicle instance with the specified properties and related entities without saving it (Vehicle) to the database. 
    /// This method still creates the related entities in the database context to ensure that all foreign key references are valid, but it does not save the Vehicle itself.
    /// </summary>
    /// <param name="context">The rental context to use for creating related entities.</param>
    /// <returns>The built Vehicle instance as Task<Vehicle></returns>
    public async Task<Vehicle> BuildUnsavedAsync(RentalContext context)
    {
        return await BuildAsync(context);
    }

    /// <summary>
    /// Private method to build the Vehicle instance with the specified properties and related entities. 
    /// This method handles the creation and saving of related entities in the database context to ensure that all foreign key references are valid when constructing the Vehicle. 
    /// </summary>
    /// <param name="context">The rental context to use for creating related entities.</param>
    /// <returns>The built Vehicle instance as Task<Vehicle></returns>
    private async Task<Vehicle> BuildAsync(RentalContext context)
    {
        // create and save related entities
        var brand = await context.VehicleBrands.FirstOrDefaultAsync( b => b.Name == _brandName); 
        if (brand == null)
        {
            brand = new VehicleBrand { Name = _brandName };
            context.VehicleBrands.Add(brand);
            await context.SaveChangesAsync();
        }

        var model = await context.VehicleModels.FirstOrDefaultAsync(m => m.Name == _modelName && m.VehicleBrandId == brand.Id);
        if (model == null)
        {
            model = new VehicleModel { Name = _modelName, VehicleBrandId = brand.Id };
            context.VehicleModels.Add(model);
            await context.SaveChangesAsync();
        }

        var color = await context.VehicleColors.FirstOrDefaultAsync(c => c.Name == _colorName);
        if (color == null)
        {
            color = new VehicleColor { Name = _colorName, HtmlColor = _colorHtml };
            context.VehicleColors.Add(color);
            await context.SaveChangesAsync();
        }

        var body = await context.VehicleBodies.FirstOrDefaultAsync(b => b.Name == _bodyName);
        if (body == null)
        {
            body = new VehicleBody { Name = _bodyName };
            context.VehicleBodies.Add(body);
            await context.SaveChangesAsync();
        }

        return new Vehicle
        {
            LicensePlate = _licensePlate,
            Mileage = _mileage,
            VehicleBrandId = brand.Id,
            Horsepower = _horsepower,
            VehicleModelId = model.Id,
            VehicleColorId = color.Id,
            VehicleBodyId = body.Id,
        };
    }

}