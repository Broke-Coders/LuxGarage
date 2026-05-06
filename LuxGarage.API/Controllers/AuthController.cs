using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LuxGarage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("regiseter")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result)
            {
                return BadRequest($"Employee with Login {request.Login} already exists.");
            }
            
            return Ok("Registration finished successfully.");
        }
    }
}
