using Common.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class MassTransitExtensions
{
    public static void AddRabbitMq(this IServiceCollection services, IConfigurationManager configuration, System.Reflection.Assembly assembly)
    {
        services.AddMassTransit(x =>
        {
            var rabbitMqOptions = new RabbitMqOptions();
            configuration.GetRequiredSection(RabbitMqOptions.Key).Bind(rabbitMqOptions);

            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host(rabbitMqOptions.HostUri, "/", h =>
                {
                    h.Username(rabbitMqOptions.HostPassword);
                    h.Password(rabbitMqOptions.HostUserName);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumers(assembly);
        });
    }
}
