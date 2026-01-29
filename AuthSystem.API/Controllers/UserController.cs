using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.API.DTOs;
using AuthSystem.API.Services;

namespace AuthSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Todas as rotas deste controller exigem autenticação
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/user/profile
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        // Extrai o ID do usuário do token JWT
        var userId = GetUserIdFromToken();

        if (userId == null)
        {
            return Unauthorized(new { message = "Usuário não autenticado" });
        }

        var user = await _userService.GetProfileAsync(userId.Value);

        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }

        return Ok(user);
    }

    // PUT: api/user/profile
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetUserIdFromToken();

        if (userId == null)
        {
            return Unauthorized(new { message = "Usuário não autenticado" });
        }

        var user = await _userService.UpdateProfileAsync(userId.Value, request);

        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }

        return Ok(user);
    }

    // ===== HELPER: Extrai o ID do usuário do token =====
    private int? GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return null;
        }

        return int.Parse(userIdClaim);
    }
}
