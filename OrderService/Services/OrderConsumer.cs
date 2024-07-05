using OrderService.Models;
using MassTransit;

namespace OrderService.Services;

// TODO: should be consuming a model called order item, not cart item
public class OrderConsumer : IConsumer<OrderItem>
{
    public Task Consume(ConsumeContext<OrderItem> context)
    {
        throw new NotImplementedException();
    }
}
