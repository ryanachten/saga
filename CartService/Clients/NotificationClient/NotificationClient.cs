using Microsoft.Extensions.Options;

namespace CartService.Clients.NotificationClient;

public class NotificationClient : BaseClient, INotificationClient
{
    private readonly HttpClient _httpClient;

    public NotificationClient(
        HttpClient httpClient,
        IOptions<NotificationClientSettings> settings,
        ILogger<NotificationClient> logger
    ) : base(httpClient, logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUri);
    }

    public async Task PushOrderNotification(Guid id)
    {
        var request = FormatPostRequest($"order/{id}");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
