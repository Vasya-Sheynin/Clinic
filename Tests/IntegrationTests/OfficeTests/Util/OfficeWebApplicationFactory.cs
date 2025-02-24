using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Moq;
using OfficeRepositories;
using OfficesController;
using Persistence;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace IntegrationTests.OfficeTests.Util;

internal class OfficeWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IOfficeRepository> OfficeRepoMock { get; }
    public Mock<ICachingService> CacheServiceMock { get; }

    public OfficeWebApplicationFactory()
    {
        OfficeRepoMock = new Mock<IOfficeRepository>();
        CacheServiceMock = new Mock<ICachingService>();
        Environment.SetEnvironmentVariable("ENVIRONMENT", "TEST");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(OfficeRepoMock.Object);
            services.AddSingleton(CacheServiceMock.Object);

            services.RemoveAll(typeof(SqlConnectionFactory));
            services.AddSingleton(serviceProvider =>
            {
                var connString = string.Empty;
                return new SqlConnectionFactory(connString);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "TestScheme", options => { });
        });
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Role, "Receptionist") };
        var identity = new ClaimsIdentity(claims, "TestType");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
