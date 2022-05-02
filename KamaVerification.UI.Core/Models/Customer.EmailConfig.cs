namespace KamaVerification.UI.Core.Models
{
    public class CustomerEmailConfig
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid PublicKey { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public int ExpirationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
