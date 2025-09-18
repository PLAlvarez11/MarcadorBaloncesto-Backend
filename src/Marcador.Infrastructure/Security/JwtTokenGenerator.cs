using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marcador.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Marcador.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;
    public JwtTokenGenerator(JwtOptions options) => _options = options;

    public string GenerateToken(Usuario user, string rolNombre, IEnumerable<string> accesos)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.NombreUsuario),
            new(ClaimTypes.Role, rolNombre)
        };

        foreach (var a in accesos.Distinct())
            claims.Add(new Claim("access", a));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
