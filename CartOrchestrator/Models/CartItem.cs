namespace CartOrchestrator.Models;

public class CartItem
{
    public string Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
