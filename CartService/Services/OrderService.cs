using CartService.Clients.DairyClient;
using CartService.Clients.DeliveryClient;
using CartService.Clients.NotificationClient;
using CartService.Clients.ProduceClient;
using CartService.Models;
using CartService.Models.Enums;

namespace CartService.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IDairyClient _dairyClient;
    private readonly IProduceClient _produceClient;
    private readonly IDeliveryClient _deliveryClient;
    private readonly INotificationClient _notificationClient;

    public OrderService(
        ILogger<OrderService> logger,
        IDairyClient dairyClient,
        IProduceClient produceClient,
        IDeliveryClient deliveryClient,
        INotificationClient notificationClient
    )
    {
        _logger = logger;
        _dairyClient = dairyClient;
        _produceClient = produceClient;
        _deliveryClient = deliveryClient;
        _notificationClient = notificationClient;
    }

    public async Task SubmitOrder(IEnumerable<CartItem> items)
    {
        _logger.LogInformation("**** Submitting order ****");
        foreach (var item in items) _logger.LogInformation("item: {item.Name} count: {item.Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(items);
        await _dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(items);
        await _produceClient.SaveOrder(produceItems);

        var orderId = await _deliveryClient.SaveOrder(GetDeliveryOrder(items));

        await _notificationClient.PushOrderNotification(orderId);
    }

    public async Task SubmitOrchestratedOrder(IEnumerable<CartItem> items)
    {
        _logger.LogInformation("**** Submitting order ****");
        foreach (var item in items) _logger.LogInformation("item: {item.Name} count: {item.Count}", item.Name, item.Count);

        // We don't need to perform any compensating transactions in this failure case
        // given there are no prior transactions to rollback
        var dairyItems = GetDairyItems(items);
        await _dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(items);
        try
        {
            await _produceClient.SaveOrder(produceItems);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error submitting produce: {ex.Message}", ex.Message);

            // In the case of an error scenario
            // we need to use compensating transactions to rollback prior transactions
            await _dairyClient.DeleteOrder(dairyItems);

            throw;
        }

        Guid orderId;
        try
        {
            orderId = await _deliveryClient.SaveOrder(GetDeliveryOrder(items));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error submitting delivery: {ex.Message}", ex.Message);

            // In the case of an error scenario
            // we need to use compensating transactions to rollback prior transactions
            await _dairyClient.DeleteOrder(dairyItems);
            await _produceClient.DeleteOrder(produceItems);

            throw;
        }

        await _notificationClient.PushOrderNotification(orderId);
    }

    private static IEnumerable<DairyItem> GetDairyItems(IEnumerable<CartItem> items)
        => items.Where(x => x.Type == ItemType.DAIRY).Select(x => new DairyItem()
        {
            Name = x.Name,
            Count = x.Count
        });

    private static IEnumerable<ProduceItem> GetProduceItems(IEnumerable<CartItem> items)
        => items.Where(x => x.Type == ItemType.PRODUCE).Select(x => new ProduceItem()
        {
            Name = x.Name,
            Count = x.Count
        });

    private static DeliveryOrder GetDeliveryOrder(IEnumerable<CartItem> items)
        => new()
        {
            Items = items.Select(x => new DeliveryItem() { Name = x.Name, Count = x.Count })
        };
}
