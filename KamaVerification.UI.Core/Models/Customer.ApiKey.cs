namespace KamaVerification.UI.Core.Models
{
    public class CustomerApiKey
    {
        public Guid? PublicKey { get; set; }
        public string? ApiKey { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
