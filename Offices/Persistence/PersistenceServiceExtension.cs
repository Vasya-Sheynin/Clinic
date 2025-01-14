using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class PersistenceServiceExtension
{
    public static void AddSqlConnectionFactory(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            var connString = Environment.GetEnvironmentVariable("DB_CONNECTION") ??
                throw new ApplicationException("Db connection string is empty"); 

            return new SqlConnectionFactory(connString);
        });
    }
}
