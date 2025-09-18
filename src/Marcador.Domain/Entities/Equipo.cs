using Marcador.Domain.Common;

namespace Marcador.Domain.Entities;

public class Equipo : AuditableEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public ICollection<Jugador> Jugadores { get; set; } = new List<Jugador>();
    public Logo? Logo { get; set; }
    public ICollection<Partido> PartidosComoLocal { get; set; } = new List<Partido>();
    public ICollection<Partido> PartidosComoVisitante { get; set; } = new List<Partido>();
}
