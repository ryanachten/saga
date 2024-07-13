using Common.Contracts.Enums;

namespace Common.Contracts;

public class OrderItem
{
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required int Count { get; set; }
}
