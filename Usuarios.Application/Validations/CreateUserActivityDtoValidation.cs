using FluentValidation;

using Usuarios.Application.DTOs;

namespace Usuarios.Application.Validations
{
    public class CreateUserActivityDtoValidation : AbstractValidator<CreateUserActivityDto>
    {
        public CreateUserActivityDtoValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El UserId es obligatorio.")
                .Matches(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("La acción es obligatoria.")
                .MaximumLength(100).WithMessage("La acción no debe superar los 100 caracteres.");
        }
    }
}
