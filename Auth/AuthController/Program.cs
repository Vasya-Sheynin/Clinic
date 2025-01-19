using Application.Dto;
using Application.Validation;
using Auth.Defaults;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.AuthService;
using Infrastructure.EmailService;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Domain;

namespace Auth;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureSwagger();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UsersDbContext>();

        builder.Services.ConfigurePersistence();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJWT(builder.Configuration);

        builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.ConfigureMassTransit();

        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailConfiguration"));

        builder.Services.AddExceptionHandling(builder.Environment);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigration();
        }

        app.UseProblemDetails();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await DefaultValuesProvider.SeedApplication(app);

        app.Run();
    }
}
