namespace Marcador.Application.DTOs.Logos;

public class LogoUploadDto
{
    public int EquipoId { get; set; }
    public string FileType { get; set; } = null!;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
}
