using Microsoft.Extensions.Options;

namespace CartOrchestrator.Clients.ProduceClient;

public class ProduceClient : BaseClient, IProduceClient
{
    private readonly HttpClient _httpClient;

    public ProduceClient(HttpClient httpClient, IOptions<ProduceClientSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUri);
    }

    public async Task SaveOrder(IEnumerable<ProduceItem> items)
    {
        var request = FormatPostRequest(items, "order");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
