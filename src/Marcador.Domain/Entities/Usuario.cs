using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Usuario : AuditableEntity
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RolId { get; set; }
    public Rol? Rol { get; set; }
}
