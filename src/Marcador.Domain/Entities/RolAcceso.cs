using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class RolAcceso : AuditableEntity
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public Rol? Rol { get; set; }
    public int AccesoId { get; set; }
    public Acceso? Acceso { get; set; }
}
