using LuxGarage.API.DTOs.Requests;
using LuxGarage.API.DTOs.Responses;
using LuxGarage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LuxGarage.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related endpoints, including user registration and login.
    /// </summary>
    /// <remarks>
    /// The AuthController provides endpoints for user registration and login, 
    /// utilizing the IAuthService to perform the necessary business logic.
    /// The controller includes proper error handling and returns appropriate HTTP status codes based on the outcome of the
    /// operations, such as 201 Created for successful registration, 400 Bad Request for invalid input,
    /// 409 Conflict for existing users during registration, and 401 Unauthorized for failed login attempts.
    /// Additionally, it handles unexpected exceptions by returning a 500 Internal Server Error with a relevant message
    /// to ensure that clients receive informative responses in case of errors.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the AuthController class with the specified authentication service.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="request">The registration request.</param>
        /// <returns>The result of the registration operation.</returns>
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

        /// <summary>
        /// Authenticates a user with the provided login credentials and returns a JWT token if successful.
        /// </summary>
        /// <param name="request">The login request.</param>
        /// <returns>The result of the login operation.</returns>
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
