using FluentValidation;
using Marcador.Application.DTOs.Partidos;

namespace Marcador.Application.Validation;

public class PartidoUpdateValidator : AbstractValidator<PartidoUpdateDto>
{
    public PartidoUpdateValidator()
    {
        RuleFor(x => x.PuntajeEquipo1)
            .GreaterThanOrEqualTo(0).WithMessage("El puntaje no puede ser negativo");

        RuleFor(x => x.PuntajeEquipo2)
            .GreaterThanOrEqualTo(0).WithMessage("El puntaje no puede ser negativo");

        RuleFor(x => x.FaltasEquipo1)
            .GreaterThanOrEqualTo(0).WithMessage("Las faltas no pueden ser negativas");

        RuleFor(x => x.FaltasEquipo2)
            .GreaterThanOrEqualTo(0).WithMessage("Las faltas no pueden ser negativas");

        RuleFor(x => x.CuartoActual)
            .InclusiveBetween(1, 4).WithMessage("El cuarto debe estar entre 1 y 4");
    }
}
