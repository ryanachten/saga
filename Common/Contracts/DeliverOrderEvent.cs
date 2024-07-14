using Common.Models;

namespace Common.Contracts;

public class DeliverOrderEvent(Guid orderId, Recipient recipient)
{
    public Guid OrderId => orderId;
    public Recipient Recipient => recipient;
}
