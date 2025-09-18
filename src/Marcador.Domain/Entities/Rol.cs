using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Rol : AuditableEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<RolAcceso> RolAccesos { get; set; } = new List<RolAcceso>();
}
