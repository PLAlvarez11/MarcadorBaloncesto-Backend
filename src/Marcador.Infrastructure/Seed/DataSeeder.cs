using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(MarcadorDbContext db, string adminUser = "admin", string adminPwdHash = null!)
    {
        // Accesos base sugeridos
        var accesos = new[]
        {
            "Equipos.Read","Equipos.Write",
            "Jugadores.Read","Jugadores.Write",
            "Partidos.Read","Partidos.Write"
        };

        if (!await db.Accesos.AnyAsync())
        {
            foreach (var a in accesos)
                db.Accesos.Add(new Acceso { NombreAcceso = a });
        }

        // Rol ADMIN
        var rolAdmin = await db.Roles.FirstOrDefaultAsync(r => r.Nombre == "ADMIN");
        if (rolAdmin is null)
        {
            rolAdmin = new Rol { Nombre = "ADMIN" };
            db.Roles.Add(rolAdmin);
            await db.SaveChangesAsync();
        }

        // Vincular accesos a ADMIN
        var allAccesos = await db.Accesos.ToListAsync();
        foreach (var a in allAccesos)
        {
            if (!await db.RolAccesos.AnyAsync(ra => ra.RolId == rolAdmin.Id && ra.AccesoId == a.Id))
                db.RolAccesos.Add(new RolAcceso { RolId = rolAdmin.Id, AccesoId = a.Id });
        }

        // Usuario admin si no existe
        if (!await db.Usuarios.AnyAsync(u => u.NombreUsuario == adminUser))
        {
            // adminPwdHash: pásalo ya hasheado desde la capa API usando PasswordHasher, o temporalmente colócalo aquí.
            db.Usuarios.Add(new Usuario
            {
                NombreUsuario = adminUser,
                PasswordHash = adminPwdHash ?? "SET_HASHED_PASSWORD",
                RolId = rolAdmin.Id
            });
        }

        await db.SaveChangesAsync();
    }
}
