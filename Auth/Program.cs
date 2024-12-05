using Auth.Defaults;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.AuthManager;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.SessionStorageManager;
using Microsoft.AspNetCore.Identity;
using Users;

namespace Auth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersDbContext>();

            builder.Services.ConfigurePersistence(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            builder.Services.AddSingleton<ISessionStorageManager, SessionStorageManager>();

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
}
