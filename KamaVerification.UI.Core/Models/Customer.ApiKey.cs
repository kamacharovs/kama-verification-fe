namespace KamaVerification.UI.Core.Models
{
    public class CustomerApiKey
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid PublicKey { get; set; }
        public string ApiKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsEnabled { get; set; } = true;
    }
}
