using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Acceso : AuditableEntity
{
    public int Id { get; set; }
    public string NombreAcceso { get; set; } = null!;
    public ICollection<RolAcceso> RolAccesos { get; set; } = new List<RolAcceso>();
}
