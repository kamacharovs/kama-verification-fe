using System.Net.Http.Headers;
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
            string? token = null,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{path}")
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseBodyDes = JsonSerializer.Deserialize<T>(responseBody, jsonSerializerOptions ?? _serializerOptions);

            return responseBodyDes;
        }

        public async Task<T> PostAsync<T, TDto>
            (string path, 
            TDto dto,
            JsonSerializerOptions? serializeJsonSerializerOptions = null,
            JsonSerializerOptions? deserializeJsonSerializerOptions = null)
            where T : class
            where TDto : class
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{path}"),
                Content = new StringContent(JsonSerializer.Serialize(dto, deserializeJsonSerializerOptions ?? _serializerOptions), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseBodyDes = JsonSerializer.Deserialize<T>(responseBody, deserializeJsonSerializerOptions ?? _serializerOptions);

            return responseBodyDes;
        }
    }
}
