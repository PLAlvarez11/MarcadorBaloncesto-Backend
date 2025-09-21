using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class RefreshToken : AuditableEntity
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expira { get; set; }
    public bool Revocado { get; set; } = false;

    public Usuario Usuario { get; set; } = null!;
}
