using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Logo : AuditableEntity
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo? Equipo { get; set; }

    public byte[] FileData { get; set; } = Array.Empty<byte>();
    public string FileType { get; set; } = "image/png";
}
