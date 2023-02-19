namespace CartService.Models;

public class CartItem
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
