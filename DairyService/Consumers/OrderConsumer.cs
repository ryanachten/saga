using Common.Contracts;
using MassTransit;

namespace DairyService.Consumers;

public class OrderConsumer : IConsumer<Order>
{
    public Task Consume(ConsumeContext<Order> context)
    {
        throw new NotImplementedException();
    }
}
