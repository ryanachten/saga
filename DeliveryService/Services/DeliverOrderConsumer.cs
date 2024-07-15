using Common.Contracts;
using MassTransit;

namespace DeliveryService.Services;

public class DeliverOrderConsumer(ILogger<DeliverOrderConsumer> logger, IPublishEndpoint publishEndpoint) : IConsumer<DeliverOrderEvent>
{
    public Task Consume(ConsumeContext<DeliverOrderEvent> context)
    {
        logger.LogInformation("Dispatching order delivery: {Id}", context.Message.OrderId);

        return publishEndpoint.Publish(new OrderDeliveredEvent(context.Message.OrderId));
    }
}
