using Marcador.Application.DTOs.Jugadores;

namespace Marcador.Application.Abstractions.Services;

public interface IJugadorService
{
    Task<IEnumerable<JugadorDto>> GetAllAsync();
    Task<IEnumerable<JugadorDto>> GetByEquipoAsync(int equipoId);
    Task<JugadorDto?> GetByIdAsync(int id);
    Task<JugadorDto> CreateAsync(JugadorCreateDto dto);
    Task<bool> UpdateAsync(int id, JugadorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
