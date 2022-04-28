using System.ComponentModel.DataAnnotations;

namespace KamaVerification.UI.Core.Models
{
    public class CustomerFind
    {
        [Required]
        [StringLength(200, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }

        public string? Result { get; set; }
    }
}
