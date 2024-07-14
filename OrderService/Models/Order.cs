using Common.Models;

namespace OrderService.Models;

public class Order
{
    public required Recipient Recipient { get; set; }
    public required List<OrderItem> Items { get; set; }
}
