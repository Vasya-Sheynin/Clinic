using Application.Commands.ReceptionistCommands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Receptionist;

public class CreateReceptionistValidator : AbstractValidator<CreateReceptionistCommand>
{
    public CreateReceptionistValidator()
    {
        RuleFor(x => x.CreateReceptionistDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters.");

        RuleFor(x => x.CreateReceptionistDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
            .WithMessage("Middle Name must contain only letters.");

        RuleFor(x => x.CreateReceptionistDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters.");
    }
}
