using KamaVerification.UI.Core.Models;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        Task CreateAsync(CustomerDto customerDto);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }

        public async Task CreateAsync(CustomerDto customerDto)
        {
            _logger.LogInformation("Created Customer");
        }
    }
}
