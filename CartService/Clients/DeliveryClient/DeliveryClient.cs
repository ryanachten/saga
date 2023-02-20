using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CartService.Clients.DeliveryClient;

public class DeliveryClient : BaseClient, IDeliveryClient
{
    private readonly HttpClient _httpClient;

    public DeliveryClient(
        HttpClient httpClient,
        IOptions<DeliveryClientSettings> settings,
        ILogger<DeliveryClient> logger
    ) : base(httpClient, logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUri);
    }

    public async Task<Guid> SaveOrder(DeliveryOrder order, Func<Task>? fallback)
    {
        var request = FormatPostRequest(order, "order");
        var response = await SendCompensableRequest(request, fallback);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
        using var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<Guid>(responseStream);
    }

    public async Task DeleteOrder(Guid id)
    {
        var request = FormatDeleteRequest($"order/{id}");
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
