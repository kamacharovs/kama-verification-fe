using Microsoft.Extensions.Options;

namespace KamaVerification.UI.Core.Extensions
{
    public static class ServiceCollectionKamaVerificationHttpClient
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<HttpClient>();

            return services;
        }
    }
}
