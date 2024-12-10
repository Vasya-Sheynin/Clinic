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
    }
}
