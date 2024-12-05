using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users;

namespace Infrastructure.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder builder)
        {
            using IServiceScope scope = builder.ApplicationServices.CreateScope();
            using UsersDbContext context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            context.Database.Migrate();
        }
    }
}
