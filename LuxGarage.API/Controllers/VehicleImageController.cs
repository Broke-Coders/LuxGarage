using LuxGarage.API.Data;
using LuxGarage.API.Features.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Features.Cars;

[ApiController]
[Route("api/[controller]")]
public class VehicleImageController : ControllerBase
{
    private readonly VehicleImageService _imageService;
    private readonly RentalContext _context;
    private readonly string _uploadFolder;

    public VehicleImageController(VehicleImageService imageService, RentalContext context, IWebHostEnvironment env)
    {
        _imageService = imageService;
        _context = context;
        _uploadFolder = Path.Combine(env.ContentRootPath, "uploads", "cars");
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<ActionResult<List<VehicleImageResponse>>> GetByVehicleId(int vehicleId)
    {
        return Ok(await _imageService.GetByVehicleIdAsync(vehicleId));
    }

    [HttpPost]
    [Consumes("multipart/form-data")] 
    public async Task<ActionResult<List<VehicleImageResponse>>> Upload([FromForm] UploadVehicleImagesRequest request)
    {
        try
        {
            var result = await _imageService.UploadImagesAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPut("vehicle/{vehicleId}/primary/{imageId}")]
    public async Task<ActionResult> SetPrimary(int vehicleId, int imageId)
    {
        try
        {
            await _imageService.SetPrimaryAsync(vehicleId, imageId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPut("reorder")]
    public async Task<ActionResult> Reorder([FromBody] ReorderImagesRequest request)
    {
        try
        {
            await _imageService.ReorderAsync(request);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _imageService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/file")]
    public async Task<IActionResult> GetFile(int id)
    {
        var image = await _context.VehicleImages.FindAsync(id);
        if (image == null) return NotFound();

        var filePath = Path.Combine(_uploadFolder, image.VehicleId.ToString(), image.StorageKey);
        
        if (!System.IO.File.Exists(filePath)) return NotFound();

        return PhysicalFile(filePath, image.ContentType);
    }
}