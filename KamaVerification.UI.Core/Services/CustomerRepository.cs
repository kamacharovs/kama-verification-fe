using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        Task<string> FindAsync(string name);
        Task<string> CreateAsync(CustomerCreate customerCreate);
    }

    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            ILogger<CustomerRepository> logger, 
            HttpClient httpClient)
            : base(httpClient)
        {
            _logger = logger;
        }

        public async Task<string> FindAsync(string name)
        {
            return await base.GetAsync($"v1/customer/{name}");
        }

        public async Task<string> CreateAsync(CustomerCreate customerCreate)
        {
            if (customerCreate.EmailConfig?.Subject is null
                || customerCreate.EmailConfig?.FromEmail is null
                || customerCreate.EmailConfig?.FromName is null
                || customerCreate.EmailConfig?.ExpirationInMinutesStr is null)
                customerCreate.EmailConfig = null!;

            return await base.PostAsync<CustomerCreate>("v1/customer", customerCreate);
        }
    }
}
