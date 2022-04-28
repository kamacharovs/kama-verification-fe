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

        public async Task<T> GetAsync<T>(string path)
        {
            var res = await _httpClient.GetStringAsync(path);

            return JsonSerializer.Deserialize<T>(res);
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
