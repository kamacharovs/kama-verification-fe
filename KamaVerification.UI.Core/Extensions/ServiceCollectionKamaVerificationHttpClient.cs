using Microsoft.Extensions.Options;

namespace KamaVerification.UI.Core.Extensions
{
    public static class ServiceCollectionKamaVerificationHttpClient
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(config["KamaVerification:BaseUrl"]) });

            return services;
        }
    }
}
