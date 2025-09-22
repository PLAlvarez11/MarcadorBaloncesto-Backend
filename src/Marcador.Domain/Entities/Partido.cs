using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Partido : AuditableEntity
{
    public int Id { get; set; }

    public int Equipo1Id { get; set; }
    public Equipo? Equipo1 { get; set; }

    public int Equipo2Id { get; set; }
    public Equipo? Equipo2 { get; set; }

    public int PuntajeEquipo1 { get; set; }
    public int PuntajeEquipo2 { get; set; }

    public int FaltasEquipo1 { get; set; }
    public int FaltasEquipo2 { get; set; }

    public int CuartoActual { get; set; }
    public DateTime FechaPartido { get; set; }
    
     public bool Terminado { get; set; } = false;
}
