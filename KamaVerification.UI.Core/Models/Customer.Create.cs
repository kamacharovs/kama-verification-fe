using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using KamaVerification.UI.Core.Extensions;

namespace KamaVerification.UI.Core.Models
{
    public class CustomerCreate
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [Required]
        [JsonIgnore]
        public string? GenerateApiKeyStr { get; set; } = "Yes";

        public bool? GenerateApiKey => GenerateApiKeyStr?.ToLower() == "yes" ? true : false;

        [JsonPropertyName("emailConfig")]
        public EmailConfig EmailConfig { get; set; } = new EmailConfig();

        [JsonIgnore]
        public string? Result { get; set; }
    }

    public class EmailConfig
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        [JsonPropertyName("fromEmail")]
        public string? FromEmail { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        [JsonPropertyName("fromName")]
        public string? FromName { get; set; }


        [Required]
        [JsonIgnore]
        public string ExpirationInMinutesStr { get; set; } = "15";

        [JsonPropertyName("expirationInMinutes")]
        public int? ExpirationInMinutes => string.IsNullOrWhiteSpace(ExpirationInMinutesStr) ? int.Parse(ExpirationInMinutesStr) : null;
    }
}
