using Application.Mapping;
using Application.Queries.DoctorQueries;
using Infrastructure.Persistence;
using System.Reflection;
using Application.Validation;
using MediatR;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.RabbitMq;

namespace ProfilesController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.ConfigureSwagger();

        builder.Services.ConfigurePersistence();
        builder.Services.ConfigureRepoInterfaceProviders();

        builder.Services.ConfigureJWT(builder.Configuration);

        builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetDoctorProfilesQuery))));
        builder.Services.AddValidators();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddAutoMapper(typeof(DoctorMappingProfile));

        builder.Services.ConfigureMassTransit();

        builder.Services.AddExceptionHandling(builder.Environment);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapOpenApi();
        }

        app.ApplyMigration();

        app.UseProblemDetails();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
