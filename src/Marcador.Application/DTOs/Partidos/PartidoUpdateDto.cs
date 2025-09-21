namespace Marcador.Application.DTOs.Partidos;

public class PartidoUpdateDto
{
    public int PuntajeEquipo1 { get; set; }
    public int PuntajeEquipo2 { get; set; }
    public int FaltasEquipo1 { get; set; }
    public int FaltasEquipo2 { get; set; }
    public int CuartoActual { get; set; }
}
