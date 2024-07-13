using Common.Contracts;
using OrderService.Clients.DairyClient;
using OrderService.Clients.DeliveryClient;
using OrderService.Clients.NotificationClient;
using OrderService.Clients.ProduceClient;

namespace OrderService.Strategies.OrderStrategy;

public interface IOrchestratedOrderStrategy
{
    Task SubmitOrder(Order order);
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
    public async Task SubmitOrder(Order order)
    {
        // TODO: refactor to use full contract across stack
        logger.LogInformation("**** Submitting order ****");
        foreach (var item in order.Items) logger.LogInformation("item: {Name} count: {Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(order.Items);
        await dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(order.Items);
        await produceClient.SaveOrder(produceItems, () => dairyClient.DeleteOrder(dairyItems));

        var orderId = await deliveryClient.SaveOrder(GetDeliveryOrder(order.Items), async () =>
        {
            await dairyClient.DeleteOrder(dairyItems);
            await produceClient.DeleteOrder(produceItems);
        });

        await notificationClient.PushOrderNotification(orderId);
    }
}
