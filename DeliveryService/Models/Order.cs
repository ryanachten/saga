namespace DeliveryService.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<DeliveryItem> Items { get; set; } = new();
    }
}
