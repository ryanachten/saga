using OrderService.Models.Enums;

namespace OrderService.Models;

public class OrderItem
{
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required int Count { get; set; }
}
