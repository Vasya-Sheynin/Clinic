using Application.Commands.ReceptionistCommands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Receptionist;

public class UpdateReceptionistValidator : AbstractValidator<UpdateReceptionistCommand>
{
    public UpdateReceptionistValidator()
    {
        RuleFor(x => x.UpdateReceptionistDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters.");

        RuleFor(x => x.UpdateReceptionistDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
           .WithMessage("Middle Name must contain only letters.");

        RuleFor(x => x.UpdateReceptionistDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters.");
    }   
}
