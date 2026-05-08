using LuxGarage.API.Features.Auth;
using Microsoft.AspNetCore.Mvc;


namespace LuxGarage.API.Controllers;

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
    private readonly AuthService _authService;

    /// <summary>
    /// Initializes a new instance of the AuthController class with the specified authentication service.
    /// </summary>
    /// <param name="authService">The authentication service.</param>
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>The result of the registration operation.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(request);
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(new { Message = e.Message });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { Message = e.Message });
        }
    }

    /// <summary>
    /// Authenticates a user with the provided login credentials and returns a JWT token if successful.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>The result of the login operation.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        
        if (result == null)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        return Ok(result);
    }
}
