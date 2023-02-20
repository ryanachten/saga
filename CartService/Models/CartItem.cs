using CartService.Models.Enums;

namespace CartService.Models;

public class CartItem
{
    public ItemType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
