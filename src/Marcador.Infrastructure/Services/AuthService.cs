using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Auth;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Marcador.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly MarcadorDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public AuthService(MarcadorDbContext db, IPasswordHasher hasher, IJwtTokenGenerator jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var user = await _db.Usuarios
            .Include(u => u.Rol!)
                .ThenInclude(r => r.RolAccesos)
                    .ThenInclude(ra => ra.Acceso)
            .FirstOrDefaultAsync(u => u.NombreUsuario == dto.Username);

        if (user is null) return null;

        if (!_hasher.Verify(dto.Password, user.PasswordHash))
            return null;

        var role = user.Rol?.Nombre ?? "";
        var accesos = user.Rol?.RolAccesos.Select(ra => ra.Acceso!.NombreAcceso) ?? Enumerable.Empty<string>();

        var accessToken = _jwt.GenerateToken(user, role, accesos);

        var refresh = new RefreshToken
        {
            UsuarioId = user.Id,
            Token = Guid.NewGuid().ToString("N"),
            Expira = DateTime.UtcNow.AddDays(7)
        };

        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Username = user.NombreUsuario,
            Role = role,
            Accesos = accesos,
            RefreshToken = refresh.Token
        };
    }
}
