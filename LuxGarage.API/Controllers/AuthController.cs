using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Services.Interfaces;
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


        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RegisterResponse>>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration data is required."));
                }

                var user = await _authService.RegisterAsync(request);

                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration failed for unknown reasons."));
                }

                var response = ApiResponse<RegisterResponse>.CreatedAt(user, "User registered successfully.");

                return CreatedAtAction(nameof(Register), new { id = user.Id }, response);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(ApiResponse<object>.Conflict(e.Message));
            }
            catch (Exception e)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An unexpected error occured while registration.");
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login data is required."));
                }

                var result = await _authService.LoginAsync(request);
                if (result == null)
                {
                    return Unauthorized(ApiResponse<object>.Error(401, "Invalid login or password."));
                }

                return Ok(ApiResponse<LoginResponse>.Ok(result, "Login successful."));
            }
            catch (Exception e)
            {
                return StatusCode(500, ApiResponse<object>.Error(500, "An error occurred during login.", e.Message));
            }
        }
    }
}
