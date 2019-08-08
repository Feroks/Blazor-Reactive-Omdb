using JE.App.Data;
using JE.Core.Options;
using JE.Infrastructure.Services;
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

        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OmdbOptions>(configuration.GetSection(nameof(OmdbOptions)));

            // Api key will not change during Runtime.
            // In order to avoid injecting IOptions<> we can simply inject instance directly
            services.AddSingleton(sp => sp.GetService<IOptions<OmdbOptions>>().Value);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // TODO: Delete 
            services.AddSingleton<WeatherForecastService>();

            // We only have 1 service, but this allows us to avoid manually writing AddSingleton for ear service we add in the future.
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IOmdbMovieService))
                .AddClasses(classes => classes.Where(x => x.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(Program))
                .AddClasses(classes => classes.Where(x => x.Name.EndsWith("ViewModel")))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }
    }
}
