using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Partidos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PartidosController : ControllerBase
{
    private readonly IPartidoService _partidos;

    public PartidosController(IPartidoService partidos)
    {
        _partidos = partidos;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Partidos.Read")]
    public async Task<IActionResult> GetById(int id)
    {
        var partido = await _partidos.GetByIdAsync(id);
        if (partido == null) return NotFound();
        return Ok(partido);
    }

    [HttpPost]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> Create(PartidoCreateDto dto)
    {
        var created = await _partidos.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}/marcador")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> UpdateMarcador(int id, PartidoUpdateDto dto)
    {
        var ok = await _partidos.UpdateMarcadorAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("historial")]
    [Authorize(Policy = "Partidos.Read")]
    public async Task<IActionResult> GetHistorial()
    {
        var result = await _partidos.GetHistorialAsync();
        return Ok(result);
    }

    [HttpPut("{id}/terminar")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> Terminar(int id)
    {
        var ok = await _partidos.MarcarTerminadoAsync(id);
        if (!ok) return NotFound();

        return Ok(new { message = "Partido terminado manualmente" });
    }
}
