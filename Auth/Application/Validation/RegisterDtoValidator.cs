using Application.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?\d+$")
            .WithMessage("Invalid Phone Number");
    }
}