using CartService.Services;
using CartService.Strategies.OrderStrategy;

namespace CartService.Extensions;

public static class OrderStrategyExtensions
{
    public static void AddOrderStrategies(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ISimpleOrderStrategy, SimpleOrderStrategy>();
        services.AddTransient<IOrchestratedOrderStrategy, OrchestratedOrderStrategy>();
        services.AddTransient<IEventOrderStrategy, EventOrderStrategy>();
    }
}
