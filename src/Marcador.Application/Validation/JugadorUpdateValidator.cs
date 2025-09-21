using FluentValidation;
using Marcador.Application.DTOs.Jugadores;

namespace Marcador.Application.Validation;

public class JugadorUpdateValidator : AbstractValidator<JugadorUpdateDto>
{
    public JugadorUpdateValidator()
    {
        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(150);

        RuleFor(x => x.Dorsal)
            .InclusiveBetween(0, 99).WithMessage("El dorsal debe estar entre 0 y 99");

        RuleFor(x => x.Posicion)
            .NotEmpty().WithMessage("La posición es obligatoria");

        RuleFor(x => x.Estatura)
            .GreaterThan(0).WithMessage("La estatura debe ser positiva");

        RuleFor(x => x.Edad)
            .InclusiveBetween(15, 50).WithMessage("La edad debe estar entre 15 y 50 años");

        RuleFor(x => x.Nacionalidad)
            .NotEmpty().WithMessage("La nacionalidad es obligatoria")
            .MaximumLength(80);

        RuleFor(x => x.EquipoId)
            .NotEqual(0).WithMessage("Debe asignarse a un equipo válido");
    }
}
