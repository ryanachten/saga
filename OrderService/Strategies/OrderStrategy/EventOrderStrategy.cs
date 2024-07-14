using Common.Contracts;
using MassTransit;
using OrderService.Models;

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
    ILogger<EventOrderStrategy> logger
) : BaseOrderStrategy, IOrderStrategy, IEventOrderStrategy
{
    public async Task SubmitOrder(Order order)
    {
        logger.LogInformation("**** Submitting order ****");
        await publishEndpoint.Publish(new CreateOrderEvent()
        { 
            OrderId = Guid.NewGuid(),
            Recipient = order.Recipient,
            Items = order.Items,
        });
    }
}
