namespace OrderService.Configuration;

public class RabbitMqOptions
{
    public const string Key = "RabbitMq";
    public string HostUri { get; set; } = string.Empty;
    public string HostUserName { get; set; } = string.Empty;
    public string HostPassword { get; set; } = string.Empty;
}
