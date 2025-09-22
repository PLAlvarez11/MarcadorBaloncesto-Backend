using Marcador.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MarcadorController : ControllerBase
{
    private readonly IPartidoService _partidos;

    public MarcadorController(IPartidoService partidos)
    {
        _partidos = partidos;
    }

    [HttpPost("{partidoId}/puntos")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> SumarPuntos(int partidoId, int equipo, int puntos)
    {
        var ok = await _partidos.SumarPuntosAsync(partidoId, equipo, puntos);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("{partidoId}/restar")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> RestarPuntos(int partidoId, int equipo, int puntos)
    {
        var ok = await _partidos.RestarPuntosAsync(partidoId, equipo, puntos);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("{partidoId}/falta")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> RegistrarFalta(int partidoId, int equipo)
    {
        var ok = await _partidos.RegistrarFaltaAsync(partidoId, equipo);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("{partidoId}/avanzar-cuarto")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> AvanzarCuarto(int partidoId)
    {
        var partido = await _partidos.GetByIdAsync(partidoId);
        if (partido == null) return NotFound();

        if (partido.CuartoActual < 4)
        {
            partido.CuartoActual++;
        }
        else
        {
            await _partidos.MarcarTerminadoAsync(partidoId);
            return Ok(new { message = "Partido finalizado automáticamente al terminar el 4º cuarto" });
        }

        await _partidos.UpdateCuartoAsync(partido.Id, partido.CuartoActual);
        return Ok(new { message = $"Avanzó al cuarto {partido.CuartoActual}" });
    }

    [HttpPost("{partidoId}/reiniciar")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> Reiniciar(int partidoId)
    {
        var ok = await _partidos.ReiniciarMarcadorAsync(partidoId);
        return ok ? Ok() : NotFound();
    }

    [HttpGet("{partidoId}")]
    [Authorize(Policy = "Partidos.Read")]
    public async Task<IActionResult> GetMarcador(int partidoId)
    {
        var marcador = await _partidos.GetMarcadorAsync(partidoId);
        if (marcador == null) return NotFound();
        return Ok(marcador);
    }

    [HttpPost("{partidoId}/finalizar")]
    [Authorize(Policy = "Partidos.Write")]
    public async Task<IActionResult> FinalizarPartido(int partidoId)
    {
        var ok = await _partidos.MarcarTerminadoAsync(partidoId);
        if (!ok) return NotFound();

        return Ok(new { message = "Partido marcado como terminado" });
    }
}
