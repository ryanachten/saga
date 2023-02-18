using System.Text;
using System.Text.Json;

namespace CartOrchestrator.Clients
{
    public abstract class BaseClient
    {
        protected HttpRequestMessage FormatPostRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Post, content, uri);

        protected HttpRequestMessage FormatDeleteRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Delete, content, uri);

        private HttpRequestMessage FormatRequest<T>(HttpMethod method, T content, string uri)
        {
            var json = JsonSerializer.Serialize(content);
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
