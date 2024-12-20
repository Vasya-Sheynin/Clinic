using Application.Commands.PatientCommands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Patient;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.CreatePatientDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters.");

        RuleFor(x => x.CreatePatientDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
            .WithMessage("Middle Name must contain only letters.");

        RuleFor(x => x.CreatePatientDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters.");

        RuleFor(x => x.CreatePatientDto.DateOfBirth)
            .LessThan(DateTime.UtcNow);
    }
}
