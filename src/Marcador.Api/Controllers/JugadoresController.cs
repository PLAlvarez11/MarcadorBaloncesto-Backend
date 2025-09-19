using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Jugadores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JugadoresController : ControllerBase
{
    private readonly IJugadorService _jugadores;

    public JugadoresController(IJugadorService jugadores)
    {
        _jugadores = jugadores;
    }

    [HttpGet]
    [Authorize(Policy = "Jugadores.Read")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _jugadores.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("by-equipo/{equipoId}")]
    [Authorize(Policy = "Jugadores.Read")]
    public async Task<IActionResult> GetByEquipo(int equipoId)
    {
        var list = await _jugadores.GetByEquipoAsync(equipoId);
        return Ok(list);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Jugadores.Read")]
    public async Task<IActionResult> GetById(int id)
    {
        var jugador = await _jugadores.GetByIdAsync(id);
        if (jugador == null) return NotFound();
        return Ok(jugador);
    }

    [HttpPost]
    [Authorize(Policy = "Jugadores.Write")]
    public async Task<IActionResult> Create(JugadorCreateDto dto)
    {
        var created = await _jugadores.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Jugadores.Write")]
    public async Task<IActionResult> Update(int id, JugadorUpdateDto dto)
    {
        var ok = await _jugadores.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Jugadores.Write")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _jugadores.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
