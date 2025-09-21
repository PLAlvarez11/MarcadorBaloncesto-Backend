namespace Marcador.Application.DTOs.Auth;

public class RefreshRequestDto
{
    public string RefreshToken { get; set; } = null!;
}

public class RefreshResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
