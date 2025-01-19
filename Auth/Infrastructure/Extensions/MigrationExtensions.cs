using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IApplicationBuilder builder)
    {
        using IServiceScope scope = builder.ApplicationServices.CreateScope();
        using UsersDbContext context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        context.Database.Migrate();
    }
}
