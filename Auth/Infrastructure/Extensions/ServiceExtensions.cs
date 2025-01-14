using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using System.Text;
using Domain;
using Infrastructure.AuthService.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.AuthService.TokenOptions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using System.Data.Common;
using MassTransit;

namespace Infrastructure.Extensions;

public static class ServiceExtensions
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
            options.ExceptionDetailsPropertyName = "Exception details";
            options.IncludeExceptionDetails = (context, exception) => environment.IsDevelopment() || environment.IsStaging();

            options.Map<BadRequestException>(exception => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Detail,
                Title = exception.Title,
                Type = exception.Type
            });

            options.Map<ValidationException>(exception => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message
            });

            options.Map<DbException>(exception => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError
            });
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 4;
            o.User.RequireUniqueEmail = true;
        });
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<UsersDbContext>();
    }

    public static void ConfigurePersistence(this IServiceCollection services)
    {
        services.AddDbContext<UsersDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            options.UseSqlServer(connectionString);
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

        services.Configure<AccessTokenOptions>(configuration.GetSection("JwtSettings"));
        services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshSettings"));
    }

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            options.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h =>
                {
                    h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USER"));
                    h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASS"));
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}
