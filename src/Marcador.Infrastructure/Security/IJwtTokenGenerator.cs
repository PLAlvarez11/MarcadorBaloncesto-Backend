using Marcador.Domain.Entities;

namespace Marcador.Infrastructure.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Usuario user, string rolNombre, IEnumerable<string> accesos);
}
