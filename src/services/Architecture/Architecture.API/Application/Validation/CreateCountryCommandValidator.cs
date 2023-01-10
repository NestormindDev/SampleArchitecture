using FluentValidation;
using Architecture.API.Application.Commands;

namespace Architecture.API.Application.Validation
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(command => command.Name).NotNull().WithMessage("No Name found");
            RuleFor(command => command.Code).NotNull().WithMessage("No Code found");
        }

    }
}
