using Common.Models.Enums;

namespace Common.Models;

public class OrderItem
{
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required int Count { get; set; }
}
