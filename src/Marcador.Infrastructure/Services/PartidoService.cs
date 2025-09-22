using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Partidos;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class PartidoService : IPartidoService
{
    private readonly MarcadorDbContext _db;

    public PartidoService(MarcadorDbContext db)
    {
        _db = db;
    }

    public async Task<PartidoDto?> GetByIdAsync(int id)
    {
        return await _db.Partidos
            .Include(p => p.Equipo1)
            .Include(p => p.Equipo2)
            .Where(p => p.Id == id)
            .Select(p => new PartidoDto
            {
                Id = p.Id,
                Equipo1Id = p.Equipo1Id,
                Equipo1Nombre = p.Equipo1!.Nombre,
                Equipo2Id = p.Equipo2Id,
                Equipo2Nombre = p.Equipo2!.Nombre,
                PuntajeEquipo1 = p.PuntajeEquipo1,
                PuntajeEquipo2 = p.PuntajeEquipo2,
                FaltasEquipo1 = p.FaltasEquipo1,
                FaltasEquipo2 = p.FaltasEquipo2,
                CuartoActual = p.CuartoActual,
                FechaPartido = p.FechaPartido
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PartidoDto> CreateAsync(PartidoCreateDto dto)
    {
        var entity = new Partido
        {
            Equipo1Id = dto.Equipo1Id,
            Equipo2Id = dto.Equipo2Id,
            PuntajeEquipo1 = 0,
            PuntajeEquipo2 = 0,
            FaltasEquipo1 = 0,
            FaltasEquipo2 = 0,
            CuartoActual = 1,
            FechaPartido = dto.FechaPartido
        };

        _db.Partidos.Add(entity);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(entity.Id) ?? throw new Exception("Error al crear partido");
    }

    public async Task<bool> UpdateMarcadorAsync(int id, PartidoUpdateDto dto)
    {
        var entity = await _db.Partidos.FindAsync(id);
        if (entity == null) return false;

        entity.PuntajeEquipo1 = dto.PuntajeEquipo1;
        entity.PuntajeEquipo2 = dto.PuntajeEquipo2;
        entity.FaltasEquipo1 = dto.FaltasEquipo1;
        entity.FaltasEquipo2 = dto.FaltasEquipo2;
        entity.CuartoActual = dto.CuartoActual;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SumarPuntosAsync(int partidoId, int equipo, int puntos)
    {
        var partido = await _db.Partidos.FindAsync(partidoId);
        if (partido == null) return false;

        if (equipo == 1) partido.PuntajeEquipo1 += puntos;
        else if (equipo == 2) partido.PuntajeEquipo2 += puntos;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestarPuntosAsync(int partidoId, int equipo, int puntos)
    {
        var partido = await _db.Partidos.FindAsync(partidoId);
        if (partido == null) return false;

        if (equipo == 1) partido.PuntajeEquipo1 = Math.Max(0, partido.PuntajeEquipo1 - puntos);
        else if (equipo == 2) partido.PuntajeEquipo2 = Math.Max(0, partido.PuntajeEquipo2 - puntos);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RegistrarFaltaAsync(int partidoId, int equipo)
    {
        var partido = await _db.Partidos.FindAsync(partidoId);
        if (partido == null) return false;

        if (equipo == 1) partido.FaltasEquipo1++;
        else if (equipo == 2) partido.FaltasEquipo2++;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AvanzarCuartoAsync(int partidoId)
    {
        var partido = await _db.Partidos.FindAsync(partidoId);
        if (partido == null) return false;

        if (partido.CuartoActual < 4) partido.CuartoActual++;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReiniciarMarcadorAsync(int partidoId)
    {
        var partido = await _db.Partidos.FindAsync(partidoId);
        if (partido == null) return false;

        partido.PuntajeEquipo1 = 0;
        partido.PuntajeEquipo2 = 0;
        partido.FaltasEquipo1 = 0;
        partido.FaltasEquipo2 = 0;
        partido.CuartoActual = 1;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<PartidoDto?> GetMarcadorAsync(int partidoId)
    {
        return await _db.Partidos
            .Include(p => p.Equipo1)
            .Include(p => p.Equipo2)
            .Where(p => p.Id == partidoId)
            .Select(p => new PartidoDto
            {
                Id = p.Id,
                Equipo1Id = p.Equipo1Id,
                Equipo1Nombre = p.Equipo1!.Nombre,
                Equipo2Id = p.Equipo2Id,
                Equipo2Nombre = p.Equipo2!.Nombre,
                PuntajeEquipo1 = p.PuntajeEquipo1,
                PuntajeEquipo2 = p.PuntajeEquipo2,
                FaltasEquipo1 = p.FaltasEquipo1,
                FaltasEquipo2 = p.FaltasEquipo2,
                CuartoActual = p.CuartoActual,
                FechaPartido = p.FechaPartido
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PartidoHistorialDto>> GetHistorialAsync()
    {
        return await _db.Partidos
            .Include(p => p.Equipo1)
            .Include(p => p.Equipo2)
            .Where(p => p.Terminado == true) // 👈 filtro explícito
            .OrderByDescending(p => p.FechaPartido)
            .Select(p => new PartidoHistorialDto
            {
                Id = p.Id,
                Equipo1 = p.Equipo1!.Nombre,
                Equipo2 = p.Equipo2!.Nombre,
                PuntajeEquipo1 = p.PuntajeEquipo1,
                PuntajeEquipo2 = p.PuntajeEquipo2,
                Fecha = p.FechaPartido
            })
            .ToListAsync();
    }

    public async Task<bool> MarcarTerminadoAsync(int partidoId)
    {
        var partido = await _db.Partidos.FirstOrDefaultAsync(p => p.Id == partidoId);
        if (partido == null) return false;

        partido.Terminado = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCuartoAsync(int partidoId, int cuarto)
    {
        var partido = await _db.Partidos.FirstOrDefaultAsync(p => p.Id == partidoId);
        if (partido == null) return false;

        partido.CuartoActual = cuarto;
        await _db.SaveChangesAsync();
        return true;
    }

}
