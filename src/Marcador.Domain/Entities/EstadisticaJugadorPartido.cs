namespace Marcador.Domain.Entities;

public class EstadisticaJugadorPartido 
{
    public int PartidoId { get; set; }
    public int JugadorId { get; set; }

    public int Puntos { get; set; } = 0;
    public int Asistencias { get; set; } = 0;
    public int RebotesOf { get; set; } = 0;
    public int RebotesDef { get; set; } = 0;
    public int Robos { get; set; } = 0;
    public int Tapones { get; set; } = 0;
    public int Faltas { get; set; } = 0;

    public Partido Partido { get; set; } = null!;
    public Jugador Jugador { get; set; } = null!;
}
