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

    /// <summary>
    /// Submits an order without transaction management
    /// </summary>
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

    /// <summary>
    /// Submits an order using saga orchestration for transaction management
    /// </summary>
    public async Task SubmitOrchestratedOrder(IEnumerable<CartItem> items)
    {
        _logger.LogInformation("**** Submitting order ****");
        foreach (var item in items) _logger.LogInformation("item: {item.Name} count: {item.Count}", item.Name, item.Count);

        var dairyItems = GetDairyItems(items);
        await _dairyClient.SaveOrder(dairyItems);

        var produceItems = GetProduceItems(items);
        await _produceClient.SaveOrder(produceItems, () => _dairyClient.DeleteOrder(dairyItems));

        var orderId = await _deliveryClient.SaveOrder(GetDeliveryOrder(items), async () =>
        {
            await _dairyClient.DeleteOrder(dairyItems);
            await _produceClient.DeleteOrder(produceItems);
        });

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
