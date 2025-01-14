using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.RabbitMq;

public static class MassTransitServiceExtension
{
    public static void ConfigureMassTransit(this IServiceCollection services)
    {        
        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            options.AddConsumer<AccountCreatedMessageConsumer>();

            options.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h =>
                {
                    h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USER"));
                    h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASS"));
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}
