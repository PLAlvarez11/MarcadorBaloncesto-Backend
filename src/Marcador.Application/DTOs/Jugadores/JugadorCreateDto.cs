namespace Marcador.Application.DTOs.Jugadores;

public class JugadorCreateDto
{
    public string NombreCompleto { get; set; } = null!;
    public int Dorsal { get; set; }
    public string Posicion { get; set; } = null!;
    public decimal Estatura { get; set; }
    public int Edad { get; set; }
    public string Nacionalidad { get; set; } = null!;
    public int EquipoId { get; set; }
}
