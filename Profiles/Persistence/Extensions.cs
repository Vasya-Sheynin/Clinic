using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.ProfileRepositories;
using ProfileRepositories;

namespace Persistence;

public static class Extensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ProfilesDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DbConnection");
            options.UseSqlServer(connectionString);
        });
    }

    public static void ConfigureRepoInterfaceProviders(this IServiceCollection services)
    {
        services.AddScoped<IDoctorProfileRepo, DoctorProfileRepo>();
        services.AddScoped<IPatientProfileRepo, PatientProfileRepo>();
        services.AddScoped<IReceptionistProfileRepo, ReceptionistProfileRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ApplyMigration(this IApplicationBuilder builder)
    {
        using IServiceScope scope = builder.ApplicationServices.CreateScope();
        using ProfilesDbContext context = scope.ServiceProvider.GetRequiredService<ProfilesDbContext>();
        context.Database.Migrate();
    }
}
