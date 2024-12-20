using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using System.Data.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.DoctorCommands;
using Application.Commands.PatientCommands;
using Application.Commands.ReceptionistCommands;
using Application.Validation.Validators.Doctor;
using Application.Validation.Validators.Patient;
using Application.Validation.Validators.Receptionist;

namespace ProfilesController;

public static class Extensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }

    public static void AddExceptionHandling(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (context, exception) => environment.IsDevelopment() || environment.IsStaging();
            options.ExceptionDetailsPropertyName = "Exception details";

            options.Map<ValidationException>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });

            options.Map<DbException>(exception => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError
            });
        });
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateDoctorCommand>, CreateDoctorValidator>();
        services.AddScoped<IValidator<UpdateDoctorCommand>, UpdateDoctorValidator>();
        services.AddScoped<IValidator<CreatePatientCommand>, CreatePatientValidator>();
        services.AddScoped<IValidator<UpdatePatientCommand>, UpdatePatientValidator>();
        services.AddScoped<IValidator<CreateReceptionistCommand>, CreateReceptionistValidator>();
        services.AddScoped<IValidator<UpdateReceptionistCommand>, UpdateReceptionistValidator>();
    }
}
