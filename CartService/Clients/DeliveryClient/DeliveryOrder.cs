namespace CartService.Clients.DeliveryClient;

public class DeliveryItem
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DeliveryOrder
{
    public IEnumerable<DeliveryItem> Items { get; set; }
}