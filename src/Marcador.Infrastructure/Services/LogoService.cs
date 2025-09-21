using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Logos;
using Marcador.Domain.Entities;
using Marcador.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Services;

public class LogoService : ILogoService
{
    private readonly MarcadorDbContext _db;

    public LogoService(MarcadorDbContext db)
    {
        _db = db;
    }

    public async Task<LogoDto?> GetByEquipoAsync(int equipoId)
    {
        var logo = await _db.Logos.FirstOrDefaultAsync(l => l.EquipoId == equipoId);
        if (logo == null) return null;

        return new LogoDto
        {
            Id = logo.Id,
            EquipoId = logo.EquipoId,
            FileType = logo.FileType,
            FileData = logo.FileData
        };
    }

    public async Task<LogoMetadataDto> UploadAsync(LogoUploadDto dto)
    {
        var existing = await _db.Logos.FirstOrDefaultAsync(l => l.EquipoId == dto.EquipoId);
        if (existing != null)
        {
            _db.Logos.Remove(existing);
            await _db.SaveChangesAsync();
        }

        var logo = new Logo
        {
            EquipoId = dto.EquipoId,
            FileType = dto.FileType,
            FileData = dto.FileData
        };

        _db.Logos.Add(logo);
        await _db.SaveChangesAsync();

        return new LogoMetadataDto
        {
            Id = logo.Id,
            EquipoId = logo.EquipoId,
            FileType = logo.FileType
        };
    }

    public async Task<bool> DeleteAsync(int equipoId)
    {
        var existing = await _db.Logos.FirstOrDefaultAsync(l => l.EquipoId == equipoId);
        if (existing == null) return false;

        _db.Logos.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
