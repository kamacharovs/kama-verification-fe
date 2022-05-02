namespace KamaVerification.UI.Core.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public Guid PublicKey { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual CustomerApiKey ApiKey { get; set; }
        public virtual CustomerEmailConfig EmailConfig { get; set; }
    }
}
