using Marcador.Domain.Common;
using Marcador.Domain.ValueObjects;

namespace Marcador.Domain.Entities;

public class Jugador : AuditableEntity
{
    public int Id { get; set; }
    public int EquipoId { get; set; }
    public Equipo? Equipo { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public int Dorsal { get; set; }
    public PosicionJugador Posicion { get; set; }
    public decimal Estatura { get; set; } 
    public int Edad { get; set; }
    public string Nacionalidad { get; set; } = null!;
}
