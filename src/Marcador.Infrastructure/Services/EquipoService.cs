using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Equipos;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class EquipoService : IEquipoService
{
    private readonly MarcadorDbContext _db;

    public EquipoService(MarcadorDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EquipoDto>> GetAllAsync()
    {
        return await _db.Equipos
            .Select(e => new EquipoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Ciudad = e.Ciudad
            })
            .ToListAsync();
    }

    public async Task<EquipoDto?> GetByIdAsync(int id)
    {
        return await _db.Equipos
            .Where(e => e.Id == id)
            .Select(e => new EquipoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Ciudad = e.Ciudad
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EquipoDto> CreateAsync(EquipoCreateDto dto)
    {
        var entity = new Equipo
        {
            Nombre = dto.Nombre,
            Ciudad = dto.Ciudad
        };

        _db.Equipos.Add(entity);
        await _db.SaveChangesAsync();

        return new EquipoDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Ciudad = entity.Ciudad
        };
    }

    public async Task<bool> UpdateAsync(int id, EquipoUpdateDto dto)
    {
        var entity = await _db.Equipos.FindAsync(id);
        if (entity == null) return false;

        entity.Nombre = dto.Nombre;
        entity.Ciudad = dto.Ciudad;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Equipos.FindAsync(id);
        if (entity == null) return false;

        _db.Equipos.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
