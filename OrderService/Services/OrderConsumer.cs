using MassTransit;
using OrderService.Models;

namespace OrderService.Services;

public class OrderConsumer : IConsumer<Order>
{
    public Task Consume(ConsumeContext<Order> context)
    {
        throw new NotImplementedException();
    }
}
