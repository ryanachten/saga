using System.Text;
using System.Text.Json;

namespace CartService.Clients
{
    public abstract class BaseClient
    {
        protected HttpRequestMessage FormatPostRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Post, content, uri);

        protected HttpRequestMessage FormatPostRequest(string uri)
            => FormatRequest(HttpMethod.Post, uri);

        protected HttpRequestMessage FormatDeleteRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Delete, content, uri);

        protected HttpRequestMessage FormatDeleteRequest(string uri)
            => FormatRequest(HttpMethod.Delete, uri);

        private HttpRequestMessage FormatRequest<T>(HttpMethod method, T content, string uri)
        {
            var json = JsonSerializer.Serialize(content);
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private HttpRequestMessage FormatRequest(HttpMethod method, string uri)
            => new HttpRequestMessage(method, uri);
    }
}
