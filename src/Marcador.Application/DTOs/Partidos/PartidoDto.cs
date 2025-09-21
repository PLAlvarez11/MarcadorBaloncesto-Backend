namespace Marcador.Application.DTOs.Partidos;

public class PartidoDto
{
    public int Id { get; set; }
    public int Equipo1Id { get; set; }
    public string Equipo1Nombre { get; set; } = null!;
    public int Equipo2Id { get; set; }
    public string Equipo2Nombre { get; set; } = null!;
    public int PuntajeEquipo1 { get; set; }
    public int PuntajeEquipo2 { get; set; }
    public int FaltasEquipo1 { get; set; }
    public int FaltasEquipo2 { get; set; }
    public int CuartoActual { get; set; }
    public DateTime FechaPartido { get; set; }
}
