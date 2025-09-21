using System.Security.Claims;
using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly ITokenService _tokens;

    public AuthController(IAuthService auth, ITokenService tokens)
    {
        _auth = auth;
        _tokens = tokens;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        if (result is null) return Unauthorized(new { message = "Credenciales inválidas" });
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        var result = await _tokens.RefreshAsync(dto);
        if (result is null) return Unauthorized(new { message = "Refresh inválido o expirado" });
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequestDto dto)
    {
        var ok = await _tokens.LogoutAsync(dto.RefreshToken);
        if (!ok) return NotFound(new { message = "Refresh no encontrado" });
        return Ok(new { message = "Sesión cerrada" });
    }

     [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? "";
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var accesos = User.Claims
            .Where(c => c.Type == "access")
            .Select(c => c.Value)
            .ToList();

        var result = new MeResponseDto
        {
            Username = username,
            Role = role,
            Accesos = accesos
        };

        return Ok(result);
    }
}
