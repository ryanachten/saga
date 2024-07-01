using CartService.Strategies;

namespace CartService.Extensions;

public static class OrderStrategyExtensions
{
    public static void AddOrderStrategies(this IServiceCollection services)
    {
        services.AddSingleton<ISimpleOrderStrategy, SimpleOrderStrategy>();
        services.AddSingleton<IOrchestratedOrderStrategy, OrchestratedOrderStrategy>();
    }
}
