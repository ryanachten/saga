using OrderService.Clients.DairyClient;
using OrderService.Clients.DeliveryClient;
using OrderService.Clients.NotificationClient;
using OrderService.Clients.ProduceClient;

namespace OrderService.Extensions;

public static class HttpClientExtensions
{
    public static void AddHttpClients(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddHttpClient<IDairyClient, DairyClient>();
        services.Configure<DairyClientSettings>(configuration.GetSection("DairyService"));

        services.AddHttpClient<IProduceClient, ProduceClient>();
        services.Configure<ProduceClientSettings>(configuration.GetSection("ProduceService"));

        services.AddHttpClient<IDeliveryClient, DeliveryClient>();
        services.Configure<DeliveryClientSettings>(configuration.GetSection("DeliveryService"));

        services.AddHttpClient<INotificationClient, NotificationClient>();
        services.Configure<NotificationClientSettings>(configuration.GetSection("NotificationService"));
    }
}
