using Common.Contracts;
using Common.Models.Enums;
using DairyService.Models;
using MassTransit;

namespace DairyService.Services;

public class FulfillOrderConsumer(ILogger<FulfillOrderConsumer> logger, IPublishEndpoint publishEndpoint) : IConsumer<FulfillDairyOrderEvent>
{
    public Task Consume(ConsumeContext<FulfillDairyOrderEvent> context)
    {
        var items = context.Message.Items.Where(x => x.Type == ItemType.DAIRY);
        
        foreach (var item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] -= item.Count;
            }
            
            logger.LogInformation("Dairy order fulfilled. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
        }

        return publishEndpoint.Publish(new DairyOrderFulfilledEvent(context.Message.OrderId));
    }
}
