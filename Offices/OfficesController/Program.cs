using Application;
using Application.Dto;
using Application.Mapper;
using Application.Validators;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using OfficeRepositories;
using Persistence;

namespace OfficesController
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.AddSqlConnectionFactory();

            builder.Services.AddAutoMapper(typeof(OfficeMapper));

            builder.Services.AddScoped<IValidator<CreateOfficeDto>, CreateOfficeDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateOfficeDto>, UpdateOfficeDtoValidator>();

            builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
            builder.Services.AddScoped<IOfficeService, OfficeService>();

            builder.Services.AddExceptionHandling(builder.Environment);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
