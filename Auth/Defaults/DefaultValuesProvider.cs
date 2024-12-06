using Microsoft.AspNetCore.Identity;
using Users;

namespace Auth.Defaults;

public static class DefaultValuesProvider
{
    public static async Task SeedApplication(WebApplication app)
    { 
        await SeedRoles(app);
        await SeedAdminAccount(app);
    }

    public static async Task SeedRoles(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new string[] { "Admin", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                if (!await rolesManager.RoleExistsAsync(role))
                {
                    await rolesManager.CreateAsync(new IdentityRole(role));
                }
            }

        }
    }

    public static async Task SeedAdminAccount(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (await userManager.FindByNameAsync(Environment.GetEnvironmentVariable("ADMIN_NAME")) == null)
            {
                var admin = new User()
                {
                    UserName = Environment.GetEnvironmentVariable("ADMIN_NAME"),
                    Email = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                    CreatedAt = DateTime.UtcNow
                };

                var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

                await userManager.CreateAsync(admin, password);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
