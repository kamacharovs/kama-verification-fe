using KamaVerification.UI.Core;
using KamaVerification.UI.Core.Options;
using KamaVerification.UI.Core.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var services = builder.Services;
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

services.Configure<KamaVerificationOptions>(x => config.GetSection(KamaVerificationOptions.Section));
services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddLogging();

await builder.Build().RunAsync();
