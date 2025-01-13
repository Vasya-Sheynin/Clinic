using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.RabbitMq;

public static class MassTransitServiceExtension
{
    public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQSettings");

        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            options.AddConsumer<AccountCreatedMessageConsumer>();

            options.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqSettings.GetSection("Host").Value, h =>
                {
                    h.Username(rabbitMqSettings.GetSection("Username").Value);
                    h.Password(rabbitMqSettings.GetSection("Password").Value);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}
