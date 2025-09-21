using Marcador.Application.DTOs.Partidos;

namespace Marcador.Application.Abstractions.Services;

public interface IPartidoService
{
    Task<IEnumerable<PartidoDto>> GetHistorialAsync();
    Task<PartidoDto?> GetByIdAsync(int id);
    Task<PartidoDto> CreateAsync(PartidoCreateDto dto);
    Task<bool> UpdateMarcadorAsync(int id, PartidoUpdateDto dto);
}
