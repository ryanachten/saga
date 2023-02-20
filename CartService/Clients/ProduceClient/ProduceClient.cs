using Microsoft.Extensions.Options;

namespace CartService.Clients.ProduceClient;

public class ProduceClient : BaseClient, IProduceClient
{
    private readonly HttpClient _httpClient;

    public ProduceClient(
        HttpClient httpClient,
        IOptions<ProduceClientSettings> settings,
        ILogger<ProduceClient> logger
    ) : base(httpClient, logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUri);
    }

    public async Task SaveOrder(IEnumerable<ProduceItem> items, Func<Task>? fallback)
    {
        var request = FormatPostRequest(items, "order");
        var response = await SendCompensableRequest(request, fallback);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task DeleteOrder(IEnumerable<ProduceItem> items)
    {
        var request = FormatDeleteRequest(items, "order");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
