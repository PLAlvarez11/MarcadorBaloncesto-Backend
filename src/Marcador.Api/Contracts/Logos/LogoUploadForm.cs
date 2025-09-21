namespace Marcador.Api.Contracts.Logos;

using Microsoft.AspNetCore.Http;

public class LogoUploadForm
{
    public int EquipoId { get; set; }
    public IFormFile File { get; set; } = null!;
}
