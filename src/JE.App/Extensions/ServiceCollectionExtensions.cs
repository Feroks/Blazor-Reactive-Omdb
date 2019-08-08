using JE.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JE.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazor(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            return services;
        }

        public static IServiceCollection AddOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OmdbOptions>(configuration.GetSection(nameof(OmdbOptions)));

            // Api key will not change during Runtime.
            // In order to avoid injecting IOptions<> we can simply inject instance directly
            services.AddSingleton(sp => sp.GetService<IOptions<OmdbOptions>>().Value);

            return services;
        }
    }
}
