using CartService.Models;
using MassTransit;

namespace CartService.Strategies.OrderStrategy;

public interface IEventOrderStrategy
{
    Task SubmitOrder(IEnumerable<CartItem> items);
}

/// <summary>
/// Submits an order without transaction management
/// </summary>
public class EventOrderStrategy(
    IBus bus,
    ILogger<SimpleOrderStrategy> logger
) : BaseOrderStrategy, IOrderStrategy, IEventOrderStrategy
{
    public async Task SubmitOrder(IEnumerable<CartItem> items)
    {
        logger.LogInformation("**** Submitting order ****");
        await bus.Publish(items.ToArray());
    }
}
