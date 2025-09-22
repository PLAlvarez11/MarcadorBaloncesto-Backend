using Marcador.Application.DTOs.Partidos;

namespace Marcador.Application.Abstractions.Services;

public interface IPartidoService
{
    Task<PartidoDto?> GetByIdAsync(int id);
    Task<PartidoDto> CreateAsync(PartidoCreateDto dto);
    Task<bool> UpdateMarcadorAsync(int id, PartidoUpdateDto dto);

    Task<bool> SumarPuntosAsync(int partidoId, int equipo, int puntos);
    Task<bool> RestarPuntosAsync(int partidoId, int equipo, int puntos);
    Task<bool> RegistrarFaltaAsync(int partidoId, int equipo);
    Task<bool> AvanzarCuartoAsync(int partidoId);
    Task<bool> ReiniciarMarcadorAsync(int partidoId);
    Task<PartidoDto?> GetMarcadorAsync(int partidoId);
    Task<IEnumerable<PartidoHistorialDto>> GetHistorialAsync();
    Task<bool> MarcarTerminadoAsync(int partidoId);
    Task<bool> UpdateCuartoAsync(int partidoId, int cuarto);

}
