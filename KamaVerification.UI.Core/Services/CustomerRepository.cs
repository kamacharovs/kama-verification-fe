using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        void SetCustomer(Customer customer);
        bool IsLoggedIn();
        Customer? Customer { get; }
        Task<Customer> FindAsync(string name);
        Task<Customer> CreateAsync(CustomerCreate customerCreate);
        Task<TokenResponse?> GetTokenAsync(TokenRequest tokenRequest);
    }

    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly ILocalStorageRepository _localStorageRepository;

        public Customer? Customer { get; private set; }

        public CustomerRepository(
            ILogger<CustomerRepository> logger, 
            ILocalStorageRepository localStorageRepository,
            HttpClient httpClient)
            : base(httpClient)
        {
            _logger = logger;
            _localStorageRepository = localStorageRepository;
        }

        public void SetCustomer(Customer customer)
        {
            this.Customer = customer;
        }

        public bool IsLoggedIn()
        {
            return Customer != null;
        }

        public async Task<TokenResponse?> GetTokenAsync(TokenRequest tokenRequest)
        {
            var tokenResponse = await base.PostAsync<TokenResponse, TokenRequest>("v1/customer/token", tokenRequest);

            if (tokenResponse?.AccessToken is not null)
            {
                Customer = await GetAsync(tokenResponse.AccessToken);

                await _localStorageRepository.SetItemAsync("customer", Customer);
            }

            return tokenResponse;
        }

        public async Task<Customer> GetAsync(string accessToken)
        {
            return await base.GetAsync<Customer>("v1/customer/me", accessToken);
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
    }
}
