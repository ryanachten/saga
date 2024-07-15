namespace Common.Contracts;

public class OrderDeliveredEvent(Guid orderId)
{
    public Guid OrderId => orderId;
}
