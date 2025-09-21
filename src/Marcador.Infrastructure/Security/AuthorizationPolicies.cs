using Microsoft.AspNetCore.Authorization;

namespace Marcador.Infrastructure.Security;

public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";

    public const string EquiposRead = "Equipos.Read";
    public const string EquiposWrite = "Equipos.Write";

    public const string JugadoresRead = "Jugadores.Read";
    public const string JugadoresWrite = "Jugadores.Write";

    public const string PartidosRead = "Partidos.Read";
    public const string PartidosWrite = "Partidos.Write";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(AdminOnly, p => p.RequireRole("ADMIN"));

        options.AddPolicy(EquiposRead, p => p.RequireClaim("access", EquiposRead));
        options.AddPolicy(EquiposWrite, p => p.RequireClaim("access", EquiposWrite));

        options.AddPolicy(JugadoresRead, p => p.RequireClaim("access", JugadoresRead));
        options.AddPolicy(JugadoresWrite, p => p.RequireClaim("access", JugadoresWrite));

        options.AddPolicy(PartidosRead, p => p.RequireClaim("access", PartidosRead));
        options.AddPolicy(PartidosWrite, p => p.RequireClaim("access", PartidosWrite));
    }
}
