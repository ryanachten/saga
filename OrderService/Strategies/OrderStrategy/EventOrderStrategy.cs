using Common.Contracts;
using MassTransit;

namespace OrderService.Strategies.OrderStrategy;

public interface IEventOrderStrategy
{
    Task SubmitOrder(Order items);
}

/// <summary>
/// Submits an order without transaction management
/// </summary>
public class EventOrderStrategy(
    IPublishEndpoint publishEndpoint,
    ILogger<SimpleOrderStrategy> logger
) : BaseOrderStrategy, IOrderStrategy, IEventOrderStrategy
{
    public async Task SubmitOrder(Order order)
    {
        logger.LogInformation("**** Submitting order ****");
        await publishEndpoint.Publish(order);
    }
}
