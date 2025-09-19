using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Equipos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class EquiposController : ControllerBase
{
    private readonly IEquipoService _equipos;

    public EquiposController(IEquipoService equipos)
    {
        _equipos = equipos;
    }

    [HttpGet]
    [Authorize(Policy = "Equipos.Read")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _equipos.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Equipos.Read")]
    public async Task<IActionResult> GetById(int id)
    {
        var equipo = await _equipos.GetByIdAsync(id);
        if (equipo == null) return NotFound();
        return Ok(equipo);
    }

    [HttpPost]
    [Authorize(Policy = "Equipos.Write")]
    public async Task<IActionResult> Create(EquipoCreateDto dto)
    {
        var created = await _equipos.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Equipos.Write")]
    public async Task<IActionResult> Update(int id, EquipoUpdateDto dto)
    {
        var ok = await _equipos.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Equipos.Write")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _equipos.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
