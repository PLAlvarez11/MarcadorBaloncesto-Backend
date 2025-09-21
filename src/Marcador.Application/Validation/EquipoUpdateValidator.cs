using FluentValidation;
using Marcador.Application.DTOs.Equipos;

namespace Marcador.Application.Validation;

public class EquipoUpdateValidator : AbstractValidator<EquipoUpdateDto>
{
    public EquipoUpdateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del equipo es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres");

        RuleFor(x => x.Ciudad)
            .NotEmpty().WithMessage("La ciudad es obligatoria")
            .MaximumLength(100).WithMessage("La ciudad no puede superar 100 caracteres");
    }
}
