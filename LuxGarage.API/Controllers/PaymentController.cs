using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LuxGarage.API.Features.Payments;

namespace LuxGarage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("process")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        try
        {
            var response = await _paymentService.ProcessPaymentAsync(request);
            if (!response.Success)
                return BadRequest(ApiResponse<PaymentResponse>.Error(400, response.Message));

            return Ok(ApiResponse<PaymentResponse>.Ok(response, "Payment processed successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Error(500, "Error during payment processing.", ex.Message));
        }
    }
}