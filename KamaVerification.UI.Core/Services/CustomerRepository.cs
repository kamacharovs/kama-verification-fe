using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        Task<string> FindAsync(string name);
        Task<object> CreateAsync(CustomerCreate customerCreate);
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

        public async Task<object> CreateAsync(CustomerCreate customerCreate)
        {
            return await base.GetAsync("200");
        }
    }
}
