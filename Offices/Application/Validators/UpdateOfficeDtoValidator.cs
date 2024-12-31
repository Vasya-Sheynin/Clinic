using Application.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class UpdateOfficeDtoValidator : AbstractValidator<UpdateOfficeDto>
{
    public UpdateOfficeDtoValidator()
    {
        RuleFor(x => x.RegistryPhoneNumber)
            .Matches(@"^\+?\d+$")
            .WithMessage("Invalid Phone Number");
    }
}
