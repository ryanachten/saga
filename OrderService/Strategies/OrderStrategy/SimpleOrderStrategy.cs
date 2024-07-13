using Common.Contracts;
using OrderService.Clients.DairyClient;
using OrderService.Clients.DeliveryClient;
using OrderService.Clients.NotificationClient;
using OrderService.Clients.ProduceClient;

namespace OrderService.Strategies.OrderStrategy;

public interface ISimpleOrderStrategy
{
    Task SubmitOrder(Order order);
}

/// <summary>
/// Submits an order without transaction management
/// </summary>
public class SimpleOrderStrategy(
    ILogger<SimpleOrderStrategy> logger,
    IDairyClient dairyClient,
    IProduceClient produceClient,
    IDeliveryClient deliveryClient,
    INotificationClient notificationClient
) : BaseOrderStrategy, IOrderStrategy, ISimpleOrderStrategy
{
    public async Task SubmitOrder(Order order)
    {
        // TODO: refactor to use full contract across stack
        logger.LogInformation("**** Submitting order ****");
        foreach (var item in order.Items) logger.LogInformation("item: {Name} count: {Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(order.Items);
        await dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(order.Items);
        await produceClient.SaveOrder(produceItems);

        var orderId = await deliveryClient.SaveOrder(GetDeliveryOrder(order.Items));

        await notificationClient.PushOrderNotification(orderId);
    }
}
