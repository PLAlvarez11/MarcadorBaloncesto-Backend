namespace Marcador.Domain.Common;

public abstract class AuditableEntity
{
    public int? UsuarioCreacionId { get; set; }
    public DateTime? FechaCreacion { get; set; }

    public int? UsuarioEdicionId { get; set; }
    public DateTime? FechaEdicion { get; set; }
}
