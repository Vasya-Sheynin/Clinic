using Application.Commands.PatientCommands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Patient;

public class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.UpdatePatientDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters.");

        RuleFor(x => x.UpdatePatientDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
            .WithMessage("Middle Name must contain only letters.");

        RuleFor(x => x.UpdatePatientDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters.");

        RuleFor(x => x.UpdatePatientDto.DateOfBirth)
            .LessThan(DateTime.UtcNow);
    }
}
