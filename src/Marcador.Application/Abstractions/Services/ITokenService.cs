using Marcador.Application.DTOs.Auth;

namespace Marcador.Application.Abstractions.Services;

public interface ITokenService
{
    Task<RefreshResponseDto?> RefreshAsync(RefreshRequestDto dto);
    Task<bool> LogoutAsync(string refreshToken);
}
