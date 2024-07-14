namespace Common.Contracts;

public class ProduceOrderFulfilledEvent(Guid orderId)
{
    public Guid OrderId => orderId;
}
