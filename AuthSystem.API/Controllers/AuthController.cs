using Microsoft.AspNetCore.Mvc;
using AuthSystem.API.DTOs;
using AuthSystem.API.Services;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    // POST: api/auth/confirm-email
    [HttpPost("confirm-email")]
    public async Task<ActionResult<AuthResponse>> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var response = await _authService.ConfirmEmailAsync(request.Token);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    // POST: api/auth/resend-confirmation
    [HttpPost("resend-confirmation")]
    public async Task<ActionResult<AuthResponse>> ResendConfirmation([FromBody] ResendConfirmationRequest request)
    {
        var response = await _authService.ResendConfirmationEmailAsync(request.Email);

        return Ok(response);
    }
}
