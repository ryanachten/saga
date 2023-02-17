using Microsoft.Extensions.Options;

namespace CartOrchestrator.Clients.DairyClient;

public class DairyClient : BaseClient, IDairyClient
{
    private readonly HttpClient _httpClient;

    public DairyClient(HttpClient httpClient, IOptions<DairyClientSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUri);
    }

    public async Task SaveOrder(IEnumerable<DairyItem> items)
    {
        var request = FormatPostRequest(items, "order");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
