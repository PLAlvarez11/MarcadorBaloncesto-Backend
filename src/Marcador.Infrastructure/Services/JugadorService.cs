using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Jugadores;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class JugadorService : IJugadorService
{
    private readonly MarcadorDbContext _db;

    public JugadorService(MarcadorDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<JugadorDto>> GetAllAsync()
    {
        return await _db.Jugadores
            .Include(j => j.Equipo)
            .Select(j => new JugadorDto
            {
                Id = j.Id,
                NombreCompleto = j.NombreCompleto,
                Dorsal = j.Dorsal,
                Posicion = j.Posicion.ToString(),
                Estatura = j.Estatura,
                Edad = j.Edad,
                Nacionalidad = j.Nacionalidad,
                EquipoId = j.EquipoId,
                EquipoNombre = j.Equipo!.Nombre
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<JugadorDto>> GetByEquipoAsync(int equipoId)
    {
        return await _db.Jugadores
            .Where(j => j.EquipoId == equipoId)
            .Include(j => j.Equipo)
            .Select(j => new JugadorDto
            {
                Id = j.Id,
                NombreCompleto = j.NombreCompleto,
                Dorsal = j.Dorsal,
                Posicion = j.Posicion.ToString(),
                Estatura = j.Estatura,
                Edad = j.Edad,
                Nacionalidad = j.Nacionalidad,
                EquipoId = j.EquipoId,
                EquipoNombre = j.Equipo!.Nombre
            })
            .ToListAsync();
    }

    public async Task<JugadorDto?> GetByIdAsync(int id)
    {
        return await _db.Jugadores
            .Include(j => j.Equipo)
            .Where(j => j.Id == id)
            .Select(j => new JugadorDto
            {
                Id = j.Id,
                NombreCompleto = j.NombreCompleto,
                Dorsal = j.Dorsal,
                Posicion = j.Posicion.ToString(),
                Estatura = j.Estatura,
                Edad = j.Edad,
                Nacionalidad = j.Nacionalidad,
                EquipoId = j.EquipoId,
                EquipoNombre = j.Equipo!.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<JugadorDto> CreateAsync(JugadorCreateDto dto)
    {
        var entity = new Jugador
        {
            NombreCompleto = dto.NombreCompleto,
            Dorsal = dto.Dorsal,
            Posicion = Enum.Parse<Domain.ValueObjects.PosicionJugador>(dto.Posicion),
            Estatura = dto.Estatura,
            Edad = dto.Edad,
            Nacionalidad = dto.Nacionalidad,
            EquipoId = dto.EquipoId
        };

        _db.Jugadores.Add(entity);
        await _db.SaveChangesAsync();

        return new JugadorDto
        {
            Id = entity.Id,
            NombreCompleto = entity.NombreCompleto,
            Dorsal = entity.Dorsal,
            Posicion = entity.Posicion.ToString(),
            Estatura = entity.Estatura,
            Edad = entity.Edad,
            Nacionalidad = entity.Nacionalidad,
            EquipoId = entity.EquipoId,
            EquipoNombre = (await _db.Equipos.FindAsync(entity.EquipoId))?.Nombre ?? ""
        };
    }

    public async Task<bool> UpdateAsync(int id, JugadorUpdateDto dto)
    {
        var entity = await _db.Jugadores.FindAsync(id);
        if (entity == null) return false;

        entity.NombreCompleto = dto.NombreCompleto;
        entity.Dorsal = dto.Dorsal;
        entity.Posicion = Enum.Parse<Domain.ValueObjects.PosicionJugador>(dto.Posicion);
        entity.Estatura = dto.Estatura;
        entity.Edad = dto.Edad;
        entity.Nacionalidad = dto.Nacionalidad;
        entity.EquipoId = dto.EquipoId;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Jugadores.FindAsync(id);
        if (entity == null) return false;

        _db.Jugadores.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
