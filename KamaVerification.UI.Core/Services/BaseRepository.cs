using System.Text;
using System.Text.Json;

namespace KamaVerification.UI.Core.Services
{
    public abstract class BaseRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public BaseRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<T> GetAsync<T>(
            string path,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{path}")
            };

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseBodyDes = JsonSerializer.Deserialize<T>(responseBody, jsonSerializerOptions ?? _serializerOptions);

            return responseBodyDes;
        }

        public async Task<string> PostAsync<TDto>(string path, TDto dto)
            where TDto : class
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{path}"),
                Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return responseBody;
        }
    }
}
