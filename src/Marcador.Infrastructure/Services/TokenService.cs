using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Auth;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Marcador.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly MarcadorDbContext _db;
    private readonly IJwtTokenGenerator _jwt;

    public TokenService(MarcadorDbContext db, IJwtTokenGenerator jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<RefreshResponseDto?> RefreshAsync(RefreshRequestDto dto)
{
    var stored = await _db.RefreshTokens
        .Include(r => r.Usuario)
            .ThenInclude(u => u.Rol)
                .ThenInclude(r => r.RolAccesos)
                    .ThenInclude(ra => ra.Acceso)
        .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && !r.Revocado);

    if (stored == null || stored.Expira < DateTime.UtcNow)
        return null;

    var role = stored.Usuario.Rol?.Nombre ?? "";
    var accesos = stored.Usuario.Rol?.RolAccesos?
        .Where(ra => ra.Acceso != null)
        .Select(ra => ra.Acceso!.NombreAcceso)
        ?? Enumerable.Empty<string>();

    var accessToken = _jwt.GenerateToken(stored.Usuario, role, accesos);

    var newRefresh = new RefreshToken
    {
        UsuarioId = stored.UsuarioId,
        Token = Guid.NewGuid().ToString("N"),
        Expira = DateTime.UtcNow.AddDays(7)
    };

    stored.Revocado = true;
    _db.RefreshTokens.Add(newRefresh);
    await _db.SaveChangesAsync();

    return new RefreshResponseDto
    {
        AccessToken = accessToken,
        RefreshToken = newRefresh.Token,
        ExpiresAt = DateTime.UtcNow.AddMinutes(60)
    };
}


    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
        if (stored == null) return false;

        stored.Revocado = true;
        await _db.SaveChangesAsync();
        return true;
    }
}
