namespace Marcador.Application.DTOs.Logos;

public class LogoDto
{
    public int Id { get; set; }
    public int EquipoId { get; set; }
    public string FileType { get; set; } = null!;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
}
