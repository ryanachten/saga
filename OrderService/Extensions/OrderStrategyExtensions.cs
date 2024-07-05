using OrderService.Strategies.OrderStrategy;

namespace OrderService.Extensions;

public static class OrderStrategyExtensions
{
    public static void AddOrderStrategies(this IServiceCollection services)
    {
        services.AddTransient<Services.IOrderService, Services.OrderService>();
        services.AddTransient<ISimpleOrderStrategy, SimpleOrderStrategy>();
        services.AddTransient<IOrchestratedOrderStrategy, OrchestratedOrderStrategy>();
        services.AddTransient<IEventOrderStrategy, EventOrderStrategy>();
    }
}
