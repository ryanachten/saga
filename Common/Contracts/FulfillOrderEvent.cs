using Common.Models;

namespace Common.Contracts;

public class FulfillOrderEvent(Guid orderId, List<OrderItem> items)
{
    public Guid OrderId => orderId;
    public List<OrderItem> Items => items;
}
