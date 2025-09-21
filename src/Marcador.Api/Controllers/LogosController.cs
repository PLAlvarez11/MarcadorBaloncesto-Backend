using Marcador.Api.Contracts.Logos;   
using Marcador.Application.Abstractions.Services;
using Marcador.Application.DTOs.Logos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marcador.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LogosController : ControllerBase
{
    private readonly ILogoService _logos;

    public LogosController(ILogoService logos)
    {
        _logos = logos;
    }

    [HttpGet("{equipoId}")]
    [Authorize(Policy = "Equipos.Read")]
    public async Task<IActionResult> GetLogo(int equipoId)
    {
        var logo = await _logos.GetByEquipoAsync(equipoId);
        if (logo == null) return NotFound();

        return File(logo.FileData, logo.FileType);
    }

 [HttpPost("upload")]
    [Authorize(Policy = "Equipos.Write")]
    [Consumes("multipart/form-data")]         
    public async Task<IActionResult> Upload([FromForm] LogoUploadForm form)
    {
        if (form.File == null || form.File.Length == 0)
            return BadRequest("Archivo inválido");

        using var ms = new MemoryStream();
        await form.File.CopyToAsync(ms);

        var dto = new LogoUploadDto
        {
            EquipoId = form.EquipoId,
            FileType = form.File.ContentType,
            FileData = ms.ToArray()
        };

        var result = await _logos.UploadAsync(dto);
        return Ok(result); 
    }


    [HttpDelete("{equipoId}")]
    [Authorize(Policy = "Equipos.Write")]
    public async Task<IActionResult> Delete(int equipoId)
    {
        var ok = await _logos.DeleteAsync(equipoId);
        if (!ok) return NotFound();
        return NoContent();
    }
}
