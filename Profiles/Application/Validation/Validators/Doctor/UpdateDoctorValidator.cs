using Application.Commands.DoctorCommands;
using Application.Dto.Doctor;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validation.Validators.Doctor;

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.UpdateDoctorDto.FirstName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("First Name must contain only letters and numbers.");

        RuleFor(x => x.UpdateDoctorDto.MiddleName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]*$");
                return regex.IsMatch(x);
            })
            .WithMessage("Middle Name must contain only letters and numbers.");

        RuleFor(x => x.UpdateDoctorDto.LastName)
            .Must(x =>
            {
                var regex = new Regex("^[a-zA-Z]+$");
                return regex.IsMatch(x);
            })
            .WithMessage("Last Name must contain only letters and numbers.");
    }
}
