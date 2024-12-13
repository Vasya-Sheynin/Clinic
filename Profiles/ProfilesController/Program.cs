using Application.Mapping;
using Application.Queries;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System.Reflection;

namespace ProfilesController
{
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

            builder.Services.ConfigurePersistence(builder.Configuration);
            builder.Services.ConfigureRepoInterfaceProviders();

            builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetDoctorProfilesQuery))));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
