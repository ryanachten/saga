using System.Text;
using System.Text.Json;

namespace CartOrchestrator.Clients
{
    public abstract class BaseClient
    {
        protected HttpRequestMessage FormatPostRequest<T>(T content, string uri)
        {
            var json = JsonSerializer.Serialize(content);
            return new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
