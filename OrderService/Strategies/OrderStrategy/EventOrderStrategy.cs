using OrderService.Models;
using MassTransit;

namespace OrderService.Strategies.OrderStrategy;

public interface IEventOrderStrategy
{
    Task SubmitOrder(IEnumerable<OrderItem> items);
}

/// <summary>
/// Submits an order without transaction management
/// </summary>
public class EventOrderStrategy(
    IBus bus,
    ILogger<SimpleOrderStrategy> logger
) : BaseOrderStrategy, IOrderStrategy, IEventOrderStrategy
{
    public async Task SubmitOrder(IEnumerable<OrderItem> items)
    {
        logger.LogInformation("**** Submitting order ****");
        await bus.Publish(items.ToArray());
    }
}
