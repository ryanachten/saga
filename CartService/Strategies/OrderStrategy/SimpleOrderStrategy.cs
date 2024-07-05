using CartService.Clients.DairyClient;
using CartService.Clients.DeliveryClient;
using CartService.Clients.NotificationClient;
using CartService.Clients.ProduceClient;
using CartService.Models;

namespace CartService.Strategies.OrderStrategy;

public interface ISimpleOrderStrategy
{
    Task SubmitOrder(IEnumerable<CartItem> items);
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
    public async Task SubmitOrder(IEnumerable<CartItem> items)
    {
        logger.LogInformation("**** Submitting order ****");
        foreach (var item in items) logger.LogInformation("item: {Name} count: {Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(items);
        await dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(items);
        await produceClient.SaveOrder(produceItems);

        var orderId = await deliveryClient.SaveOrder(GetDeliveryOrder(items));

        await notificationClient.PushOrderNotification(orderId);
    }
}
