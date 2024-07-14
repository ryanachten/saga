using Common.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Common.Extensions;

public static class MassTransitExtensions
{
    /// <summary>
    /// Extension for shared MassTransit and RabbitMQ configuration
    /// </summary>
    public static void UsingCommonRabbitMq(this IBusRegistrationConfigurator registration, IConfigurationManager configuration, Assembly assembly)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetRequiredSection(RabbitMqOptions.Key).Bind(rabbitMqOptions);

        registration.SetKebabCaseEndpointNameFormatter();
        registration.UsingRabbitMq((context, cfg) =>
        {

            cfg.Host(rabbitMqOptions.HostUri, "/", h =>
            {
                h.Username(rabbitMqOptions.HostPassword);
                h.Password(rabbitMqOptions.HostUserName);
            });

            cfg.ConfigureEndpoints(context);
        });

        registration.AddConsumers(assembly);
    }
}
