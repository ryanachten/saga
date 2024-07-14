using Common.Models;

namespace Common.Contracts;

public class NotifyCustomerEvent
{
    public required Guid OrderId { get; set; }
    public required Recipient Recipient { get; set; }
}
