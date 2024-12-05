using Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using System.Data.Common;

namespace Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
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

        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                var connectionString = config.GetConnectionString("DbConnection");
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["AccessToken"];
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
