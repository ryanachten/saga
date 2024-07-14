using System.Text;
using System.Text.Json;

namespace OrderService.Clients
{
    public abstract class BaseClient(HttpClient httpClient, ILogger<BaseClient> logger)
    {

        /// <summary>
        /// Compensable request. Will execute a compensating request to rollback prior transactions on failure.
        /// </summary>
        /// <param name="request">Compensable request to be compensated on failure</param>
        /// <param name="compensatingRequest">Compensating request to rollback prior transactions</param>
        /// <returns>Compensable request response on success</returns>
        protected async Task<HttpResponseMessage> SendCompensableRequest(HttpRequestMessage request, Func<Task>? compensatingRequest)
        {
            try
            {
                var response = await httpClient.SendAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                if (compensatingRequest != null)
                {
                    logger.LogError(ex, "Error sending request: {Message} - executing compensating request", ex.Message);

                    await compensatingRequest();
                }
                throw;
            }
        }

        protected HttpRequestMessage FormatPostRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Post, content, uri);

        protected HttpRequestMessage FormatPostRequest(string uri)
            => FormatRequest(HttpMethod.Post, uri);

        protected HttpRequestMessage FormatDeleteRequest<T>(T content, string uri)
            => FormatRequest(HttpMethod.Delete, content, uri);

        protected HttpRequestMessage FormatDeleteRequest(string uri)
            => FormatRequest(HttpMethod.Delete, uri);

        private static HttpRequestMessage FormatRequest<T>(HttpMethod method, T content, string uri)
        {
            var json = JsonSerializer.Serialize(content);
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static HttpRequestMessage FormatRequest(HttpMethod method, string uri)
            => new HttpRequestMessage(method, uri);
    }
}
