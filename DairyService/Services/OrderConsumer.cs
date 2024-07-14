using Common.Contracts;
using MassTransit;

namespace DairyService.Services;

public class OrderConsumer : IConsumer<CreateOrderEvent>
{
    public Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        throw new NotImplementedException();
    }
}
