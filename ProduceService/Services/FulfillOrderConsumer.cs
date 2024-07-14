using Common.Contracts;
using Common.Models.Enums;
using ProduceService.Models;
using MassTransit;

namespace ProduceService.Services;

public class FulfillOrderConsumer(ILogger<FulfillOrderConsumer> logger, IPublishEndpoint publishEndpoint) : IConsumer<FulfillProduceOrderEvent>
{
    public Task Consume(ConsumeContext<FulfillProduceOrderEvent> context)
    {
        var items = context.Message.Items.Where(x => x.Type == ItemType.PRODUCE);
        
        foreach (var item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] -= item.Count;
            }
            
            logger.LogInformation("Produce order fulfilled. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
        }

        return publishEndpoint.Publish(new ProduceOrderFulfilledEvent(context.Message.OrderId));
    }
}
