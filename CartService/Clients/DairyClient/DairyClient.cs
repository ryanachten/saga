using Microsoft.Extensions.Options;

namespace CartService.Clients.DairyClient;

public class DairyClient : BaseClient, IDairyClient
{
    private readonly HttpClient _httpClient;

    public DairyClient(
        HttpClient httpClient,
        IOptions<DairyClientSettings> settings,
        ILogger<DairyClient> logger
    ) : base(httpClient, logger)
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

    public async Task DeleteOrder(IEnumerable<DairyItem> items)
    {
        var request = FormatDeleteRequest(items, "order");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
