using Marcador.Application.DTOs.Logos;

namespace Marcador.Application.Abstractions.Services;

public interface ILogoService
{
    Task<LogoDto?> GetByEquipoAsync(int equipoId);
    Task<LogoMetadataDto> UploadAsync(LogoUploadDto dto);
    Task<bool> DeleteAsync(int equipoId);
}
