namespace Marcador.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public IEnumerable<string> Accesos { get; set; } = Enumerable.Empty<string>();
}