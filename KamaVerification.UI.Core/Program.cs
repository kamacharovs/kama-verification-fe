using KamaVerification.UI.Core;
using KamaVerification.UI.Core.Extensions;
using KamaVerification.UI.Core.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var services = builder.Services;
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

services.AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<ILocalStorageRepository, LocalStorageRepository>()
    .AddScoped<AuthenticationStateProvider, ApiAuthenticationRepository>()
    .AddHttpClient(config)
    .AddAuthorizationCore()
    .AddOptions()
    .AddLogging();

await builder.Build()
    .RunAsync();
