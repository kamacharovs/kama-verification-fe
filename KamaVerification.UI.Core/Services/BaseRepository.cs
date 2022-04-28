using System.Text.Json;

namespace KamaVerification.UI.Core.Services
{
    public abstract class BaseRepository
    {
        private readonly HttpClient _httpClient;

        public BaseRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string path)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{path}")
            };

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return responseBody;
        }

        public async Task<T?> PostAsync<T, TDto>(string path, TDto dto)
            where T : class
            where TDto : class
        {
            var payload = new StringContent(JsonSerializer.Serialize(dto));
            var res = await _httpClient.PostAsync(path, payload);
            var restT = JsonSerializer.Deserialize<T>(await res.Content.ReadAsStringAsync());

            return restT;
        }
    }
}
