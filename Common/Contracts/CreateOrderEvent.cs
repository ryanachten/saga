using Common.Models;

namespace Common.Contracts;

public class CreateOrderEvent
{
    public required Guid OrderId { get; set; }
    public required Recipient Recipient { get; set; }
    public required List<OrderItem> Items { get; set; }
}
