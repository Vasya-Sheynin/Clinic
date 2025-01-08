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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

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

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("JWT_KEY");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                ValidAudience = jwtSettings.GetSection("validAudience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.FromSeconds(5)
            };
        });
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateDoctorValidator).Assembly);
    }
}
