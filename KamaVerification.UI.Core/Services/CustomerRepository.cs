using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        Task<Customer> FindAsync(string name);
        Task<Customer> CreateAsync(CustomerCreate customerCreate);
        Task<TokenResponse> GetTokenAsync(TokenRequest tokenRequest);
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

        public async Task<Customer> FindAsync(string name)
        {
            return await base.GetAsync<Customer>($"v1/customer/{name}");
        }

        public async Task<Customer> CreateAsync(CustomerCreate customerCreate)
        {
            if (customerCreate.EmailConfig?.Subject is null
                || customerCreate.EmailConfig?.FromEmail is null
                || customerCreate.EmailConfig?.FromName is null
                || customerCreate.EmailConfig?.ExpirationInMinutesStr is null)
                customerCreate.EmailConfig = null!;

            return await base.PostAsync<Customer, CustomerCreate>("v1/customer", customerCreate);
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequest tokenRequest)
        {
            return await base.PostAsync<TokenResponse, TokenRequest>("v1/customer/token", tokenRequest);
        }
    }
}
