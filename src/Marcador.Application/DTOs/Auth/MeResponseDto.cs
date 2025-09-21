namespace Marcador.Application.DTOs.Auth;

public class MeResponseDto
{
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public IEnumerable<string> Accesos { get; set; } = Enumerable.Empty<string>();
}
