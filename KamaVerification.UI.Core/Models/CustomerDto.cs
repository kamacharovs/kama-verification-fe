using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using KamaVerification.UI.Core.Extensions;

namespace KamaVerification.UI.Core.Models
{
    public class CustomerDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }

        [Required]
        [JsonIgnore]
        public string? GenerateApiKeyStr { get; set; } = "Yes";
        public bool? GenerateApiKey => GenerateApiKeyStr?.ToLower() == "yes" ? true : false;

        public EmailConfigDto EmailConfig { get; set; } = new EmailConfigDto();
    }

    public class EmailConfigDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? Subject { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? FromEmail { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? FromName { get; set; }


        [Required]
        public string ExpirationInMinutesStr { get; set; } = "15";
        public int? ExpirationInMinutes => string.IsNullOrWhiteSpace(ExpirationInMinutesStr) ? int.Parse(ExpirationInMinutesStr) : null;
    }
}
