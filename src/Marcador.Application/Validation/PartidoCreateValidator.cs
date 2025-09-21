using FluentValidation;
using Marcador.Application.DTOs.Partidos;

namespace Marcador.Application.Validation;

public class PartidoCreateValidator : AbstractValidator<PartidoCreateDto>
{
    public PartidoCreateValidator()
    {
        RuleFor(x => x.Equipo1Id)
            .NotEqual(0).WithMessage("Debe seleccionar un equipo 1");

        RuleFor(x => x.Equipo2Id)
            .NotEqual(0).WithMessage("Debe seleccionar un equipo 2")
            .NotEqual(x => x.Equipo1Id).WithMessage("Equipo 1 y Equipo 2 no pueden ser iguales");

        RuleFor(x => x.FechaPartido)
            .GreaterThan(DateTime.UtcNow.AddDays(-1))
            .WithMessage("La fecha del partido debe ser futura o actual");
    }
}
