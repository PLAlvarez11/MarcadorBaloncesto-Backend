namespace Marcador.Application.DTOs.Partidos;

public class PartidoHistorialDto
{
    public int Id { get; set; }
    public string Equipo1 { get; set; } = null!;
    public string Equipo2 { get; set; } = null!;
    public int PuntajeEquipo1 { get; set; }
    public int PuntajeEquipo2 { get; set; }
    public DateTime Fecha { get; set; }
}
