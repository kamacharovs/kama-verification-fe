using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        Task<object> CreateAsync(CustomerDto customerDto);
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

        public async Task<object> CreateAsync(CustomerDto customerDto)
        {
            return await base.GetAsync<string>("200");
        }
    }
}
