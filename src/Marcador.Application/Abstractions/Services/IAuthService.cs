using Marcador.Application.DTOs.Auth;

namespace Marcador.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
}
