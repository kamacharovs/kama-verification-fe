using System.Text.Json.Serialization;

namespace KamaVerification.UI.Core.Models
{
    public class TokenRequest
    {
        [JsonPropertyName("api_key")]
        public string? ApiKey { get; set; }
    }
}
