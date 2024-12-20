using Application.Commands.DoctorCommands;
using Application.Dto.Doctor;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Doctor;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.CreateDoctorDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters.");

        RuleFor(x => x.CreateDoctorDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
            .WithMessage("Middle Name must contain only letters.");

        RuleFor(x => x.CreateDoctorDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters.");
    }
}
