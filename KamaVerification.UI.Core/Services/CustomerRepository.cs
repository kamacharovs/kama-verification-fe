﻿using KamaVerification.UI.Core.Constants;
using KamaVerification.UI.Core.Extensions;
using KamaVerification.UI.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KamaVerification.UI.Core.Services
{
    public interface ICustomerRepository
    {
        void SetCustomer(Customer customer);
        bool IsLoggedIn();
        bool IsAdmin();
        bool IsAuthorized(string? roles);
        Customer? Customer { get; }
        Task<bool> LoginAsync(TokenRequest tokenRequest);
        Task LogoutAsync();
        Task RefreshAsync();
        Task<bool> LoginEmailAsync();
        Task<Customer> FindAsync(string name);
        Task<Customer> CreateAsync(CustomerCreate customerCreate);
        Task<TokenResponse?> GetTokenAsync(TokenRequest tokenRequest);
        Task<TokenResponse?> GetEmailTokenAsync(TokenRequest tokenRequest);
    }

    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly IConfiguration _config;
        private readonly ILocalStorageRepository _localStorageRepository;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public Customer? Customer { get; private set; }

        public CustomerRepository(
            ILogger<CustomerRepository> logger, 
            IConfiguration config,
            ILocalStorageRepository localStorageRepository,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider,
            HttpClient httpClient)
            : base(httpClient)
        {
            _logger = logger;
            _config = config;
            _localStorageRepository = localStorageRepository;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        private string BaseUrl => _config["KamaVerification:BaseUrl"];
        private string BaseEmailUrl => _config["KamaVerification:BaseEmailUrl"];

        public void SetCustomer(Customer customer)
        {
            this.Customer = customer;
        }

        public bool IsLoggedIn()
        {
            return Customer != null;
        }

        public bool IsAdmin()
        {
            return Customer?.RoleName == CustomerRoles.Admin;
        }

        public bool IsAuthorized(string? roles)
        {
            if (roles is null) return true;

            return roles.Contains(Customer?.RoleName);
        }

        public async Task<bool> LoginAsync(TokenRequest tokenRequest)
        {
            var tokenResponse = await GetTokenAsync(tokenRequest);

            if (tokenResponse?.AccessToken is null) return false;
            
            Customer = await GetAsync(tokenResponse.AccessToken);

            SetCustomer(Customer);
            await _localStorageRepository.SetItemAsync("customer", Customer);
            await _localStorageRepository.SetItemAsync("customer.apikey", tokenRequest.ApiKey);
            await _localStorageRepository.SetItemAsync("customer.token", tokenResponse.AccessToken);

            ((ApiAuthenticationRepository)_authenticationStateProvider).MarkUserAsAuthenticated(tokenResponse.AccessToken);

            var navigateTo = "";
            if (_navigationManager.Uri.Contains("returnUrl")) _navigationManager.TryGetQueryString<string>("returnUrl", out navigateTo);

            _navigationManager.NavigateTo(navigateTo);

            return true;
        }

        public async Task LogoutAsync()
        {
            this.Customer = null;

            await _localStorageRepository.RemoveItemAsync("customer");
            await _localStorageRepository.RemoveItemAsync("customer.apikey");
            await _localStorageRepository.RemoveItemAsync("customer.token");

            ((ApiAuthenticationRepository)_authenticationStateProvider).MarkUserAsLoggedOut();

            _navigationManager.NavigateTo("customer/login");
        }

        public async Task RefreshAsync()
        {
            var customerApiKey = await _localStorageRepository.GetItemAsync<string>("customer.apikey");

            if (customerApiKey is not null
                && !IsLoggedIn())
            {
                await LoginAsync(new TokenRequest { ApiKey = customerApiKey });
            }
        }

        public async Task<bool> LoginEmailAsync()
        {
            if (await _localStorageRepository.GetItemAsync<string>("customer.email.token") is not null)
                return true;

            var customerApiKey = await _localStorageRepository.GetItemAsync<string>("customer.apikey");

            if (customerApiKey is null)
                return false;

            var tokenResponse = await GetEmailTokenAsync(new TokenRequest { ApiKey = customerApiKey });

            if (tokenResponse?.AccessToken is null)
                return false;

            await _localStorageRepository.SetItemAsync("customer.email.token", tokenResponse.AccessToken);

            return true;
        }

        #region API Calls
        public async Task<TokenResponse?> GetTokenAsync(TokenRequest tokenRequest)
        {
            return await base.PostAsync<TokenResponse, TokenRequest>($"{BaseUrl}/v1/customer/token", tokenRequest);
        }
        public async Task<TokenResponse?> GetEmailTokenAsync(TokenRequest tokenRequest)
        {
            return await base.PostAsync<TokenResponse, TokenRequest>($"{BaseEmailUrl}/v1/email/token", tokenRequest);
        }

        public async Task<Customer> GetAsync(string accessToken)
        {
            return await base.GetAsync<Customer>($"{BaseUrl}/v1/customer/me", accessToken);
        }

        public async Task<Customer> FindAsync(string name)
        {
            return await base.GetAsync<Customer>($"{BaseUrl}/v1/customer/{name}");
        }

        public async Task<Customer> CreateAsync(CustomerCreate customerCreate)
        {
            if (customerCreate.EmailConfig?.Subject is null
                || customerCreate.EmailConfig?.FromEmail is null
                || customerCreate.EmailConfig?.FromName is null
                || customerCreate.EmailConfig?.ExpirationInMinutesStr is null)
                customerCreate.EmailConfig = null!;

            return await base.PostAsync<Customer, CustomerCreate>($"{BaseUrl}/v1/customer", customerCreate);
        }
        #endregion
    }
}
