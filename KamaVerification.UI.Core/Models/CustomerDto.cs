using System.ComponentModel.DataAnnotations;

namespace KamaVerification.UI.Core.Models
{
    public class CustomerDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }

        public bool? GenerateApiKey { get; set; } = true;

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
        public int ExpirationInMinutes { get; set; } = 15;
    }
}
