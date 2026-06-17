using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Reports;

namespace LuxGarage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Employee")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Generates an operational report for a specific period.
    /// Access: Admin, Employee
    /// </summary>
    [HttpGet("operational")]
    public async Task<ActionResult<ApiResponse<OperationalReportResponse>>> GetOperationalReport([FromQuery] OperationalReportRequest request)
    {
        try
        {
            var report = await _reportService.GetOperationalReportAsync(request);
            return Ok(ApiResponse<OperationalReportResponse>.Ok(report, "Operational report generated successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error generating report.", ex.Message));
        }
    }
}
