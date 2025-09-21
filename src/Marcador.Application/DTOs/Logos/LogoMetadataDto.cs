namespace Marcador.Application.DTOs.Logos;

public class LogoMetadataDto
{
    public int Id { get; set; }
    public int EquipoId { get; set; }
    public string FileType { get; set; } = null!;
}
