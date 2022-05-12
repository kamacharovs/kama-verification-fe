using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Security.Principal;

namespace KamaVerification.UI.Core.Services
{
    public class ApiAuthenticationRepository : AuthenticationStateProvider
    {
        // https://chrissainty.com/securing-your-blazor-apps-authentication-with-clientside-blazor-using-webapi-aspnet-core-identity/amp/
        private readonly ILocalStorageRepository _localStorageRepository;
        private readonly HttpClient _httpClient;

        public ApiAuthenticationRepository( 
            ILocalStorageRepository localStorageRepository,
            HttpClient httpClient)
        {
            _localStorageRepository = localStorageRepository;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorageRepository.GetItemAsync<string>("customer.token");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new CustomerClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            return new AuthenticationState(new CustomerClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        public void MarkUserAsAuthenticated(string accessToken)
        {
            var authenticatedUser = new CustomerClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new CustomerClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        #region Helpers
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        #endregion
    }

    public class CustomerClaimsPrincipal : ClaimsPrincipal
    {
        public CustomerClaimsPrincipal(IIdentity identity) : base(identity) { }

        /*
         * Source Code
         * https://github.com/microsoft/referencesource/blob/master/mscorlib/system/security/claims/ClaimsPrincipal.cs#L765
         */
        public override bool IsInRole(string role)
        {
            var roleClaim = Claims.FirstOrDefault(x => x.Type == "role");

            if (roleClaim == null) return false;
            if (roleClaim.Value == role) return true;

            return false;
        }
    }
}
