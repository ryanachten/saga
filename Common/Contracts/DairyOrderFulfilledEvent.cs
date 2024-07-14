namespace Common.Contracts;

public class DairyOrderFulfilledEvent(Guid orderId)
{
    public Guid OrderId => orderId;
}
