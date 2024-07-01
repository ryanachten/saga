using CartService.Clients.DairyClient;
using CartService.Clients.DeliveryClient;
using CartService.Clients.NotificationClient;
using CartService.Clients.ProduceClient;
using CartService.Models;

namespace CartService.Strategies;

public interface IOrchestratedOrderStrategy
{
    Task SubmitOrder(IEnumerable<CartItem> items);
}

/// <summary>
/// Submits an order using saga orchestration for transaction management
/// </summary>
public class OrchestratedOrderStrategy(
    ILogger<OrchestratedOrderStrategy> logger,
    IDairyClient dairyClient,
    IProduceClient produceClient,
    IDeliveryClient deliveryClient,
    INotificationClient notificationClient
) : BaseOrderStrategy, IOrderStrategy, IOrchestratedOrderStrategy
{
    public async Task SubmitOrder(IEnumerable<CartItem> items)
    {
        logger.LogInformation("**** Submitting order ****");
        foreach (var item in items) logger.LogInformation("item: {Name} count: {Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(items);
        await dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(items);
        await produceClient.SaveOrder(produceItems, () => dairyClient.DeleteOrder(dairyItems));

        var orderId = await deliveryClient.SaveOrder(GetDeliveryOrder(items), async () =>
        {
            await dairyClient.DeleteOrder(dairyItems);
            await produceClient.DeleteOrder(produceItems);
        });

        await notificationClient.PushOrderNotification(orderId);
    }
}
