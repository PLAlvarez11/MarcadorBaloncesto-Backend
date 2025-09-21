namespace Marcador.Application.DTOs.Partidos;

public class PartidoCreateDto
{
    public int Equipo1Id { get; set; }
    public int Equipo2Id { get; set; }
    public DateTime FechaPartido { get; set; }
}
