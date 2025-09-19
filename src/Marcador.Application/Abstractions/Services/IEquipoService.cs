using Marcador.Application.DTOs.Equipos;

namespace Marcador.Application.Abstractions.Services;

public interface IEquipoService
{
    Task<IEnumerable<EquipoDto>> GetAllAsync();
    Task<EquipoDto?> GetByIdAsync(int id);
    Task<EquipoDto> CreateAsync(EquipoCreateDto dto);
    Task<bool> UpdateAsync(int id, EquipoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
